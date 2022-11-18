resource "aws_ecs_cluster" "server" {
  name = local.component_name
}

resource "aws_cloudwatch_log_group" "logs" {
  name              = "/services/${local.component_name}"
  retention_in_days = 30
}

data "aws_iam_policy_document" "execution_assume_role" {
  statement {
    effect = "Allow"
    actions = [
      "sts:AssumeRole"
    ]
    principals {
      type = "Service"
      identifiers = [
        "ecs-tasks.amazonaws.com"
      ]
    }
  }
}

resource "aws_iam_role" "execution" {
  name               = "${local.component_name}-ecs-execution"
  assume_role_policy = data.aws_iam_policy_document.execution_assume_role.json
}

data "aws_iam_policy_document" "task_assume_role" {
  statement {
    effect = "Allow"
    actions = [
      "sts:AssumeRole"
    ]
    principals {
      type = "Service"
      identifiers = [
        "ecs-tasks.amazonaws.com"
      ]
    }
  }
}

resource "aws_iam_role_policy" "execution_ecr" {
  name   = "ECR"
  role   = aws_iam_role.execution.id
  policy = data.aws_iam_policy_document.execution_ecr.json
}

data "aws_iam_policy_document" "execution_ecr" {
  statement {
    effect = "Allow"
    actions = [
      "ecr:GetAuthorizationToken",
      "ecr:BatchCheckLayerAvailability",
      "ecr:GetDownloadUrlForLayer",
      "ecr:BatchGetImage",
    ]
    resources = [
      "*"
    ]
  }
}

resource "aws_iam_role_policy" "execution_logs" {
  name   = "Logs"
  role   = aws_iam_role.execution.id
  policy = data.aws_iam_policy_document.execution_logs.json
}

data "aws_iam_policy_document" "execution_logs" {
  statement {
    effect = "Allow"
    actions = [
      "logs:PutLogEvents",
      "logs:CreateLogStream",
    ]
    resources = [
      "${aws_cloudwatch_log_group.logs.arn}:*"
    ]
  }
}


resource "aws_iam_role" "task" {
  name               = "${local.component_name}-ecs-task"
  assume_role_policy = data.aws_iam_policy_document.task_assume_role.json
}

locals {
  image_prefix = "${var.image_prefix}/${local.component_name}"
  server_container_definition = {
    essential = true
    image     = "${local.image_prefix}:${var.server_image}"
    logConfiguration = {
      logDriver = "awslogs"
      options = {
        "awslogs-group" : aws_cloudwatch_log_group.logs.name
        "awslogs-region" : data.aws_region.current.name
        "awslogs-stream-prefix" : local.component_name
      }
    }
    name = local.component_name
    portMappings = [
    {
      hostPort      = 443
      protocol      = "tcp"
      containerPort = 443
    }]
    ulimits = [
    {
      # Default for Fargate is low (1k/4k)
      # https://docs.aws.amazon.com/AmazonECS/latest/APIReference/API_Ulimit.html
      name      = "nofile"
      softLimit = 1024
      hardLimit = 65535
    }]
  }
}

resource "aws_ecs_task_definition" "server" {
  family = local.component_name

  cpu                = 256
  execution_role_arn = aws_iam_role.execution.arn
  memory             = 512
  task_role_arn      = aws_iam_role.task.arn

  container_definitions = jsonencode([
    local.server_container_definition
  ])

  network_mode = "awsvpc"

  requires_compatibilities = [
    "FARGATE"
  ]
}

resource "aws_lb_target_group" "server" {

  name_prefix = "server"
  port        = 443
  protocol    = "HTTP"
  target_type = "ip"
  vpc_id      = aws_vpc.instance.id

  health_check {
    path    = "/.healthcheck"
    matcher = "200"
  }

  lifecycle {
    create_before_destroy = true
  }
}

resource "aws_security_group" "server" {
  name        = "${local.component_name}-service"
  vpc_id      = aws_vpc.instance.id
}

resource "aws_security_group_rule" "server_egress" {
  security_group_id = aws_security_group.server.id
  type              = "egress"
  from_port         = 0
  to_port           = 0
  protocol          = "-1"
  cidr_blocks       = ["0.0.0.0/0"]
}

resource "aws_alb" "server" {
  name = local.component_name
  internal = false
  load_balancer_type = "application"

  subnets = [for subnet in aws_subnet.public : subnet.id]
  security_groups = [aws_security_group.server.id]
}

resource "aws_alb_listener" "server" {
  load_balancer_arn = aws_alb.server.arn
  port = 443
  protocol = "HTTP"
  default_action {
    type = "forward"
    target_group_arn = aws_lb_target_group.server.arn
  }
}

resource "aws_ecs_service" "server" {

  cluster         = aws_ecs_cluster.server.id
  name            = local.component_name
  task_definition = aws_ecs_task_definition.server.arn

  launch_type         = "FARGATE"
  scheduling_strategy = "REPLICA"
  desired_count       = local.desired_count

  load_balancer {
    target_group_arn = aws_lb_target_group.server.arn
    container_name   = local.component_name
    container_port   = 443
  }

  network_configuration {
    assign_public_ip = true
    security_groups = [
      aws_security_group.server.id
    ]
    subnets = [for subnet in aws_subnet.public : subnet.id]
  }
}
