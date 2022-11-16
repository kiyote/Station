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
