
data "aws_ami" "amazon_linux" {
  most_recent = true
  owners = ["amazon"]

  filter {
    name = "name"
    values = [
      local.image_filter
    ]
  }
}

resource "aws_instance" "server" {
  ami = data.aws_ami.amazon_linux.id
  instance_type = local.instance_type
  associate_public_ip_address = true
  security_groups = [
    aws_security_group.server.id
  ]

  root_block_device {
    volume_size = 1
    volume_type = "gp2"
  }
}

resource "aws_security_group" "server" {
  name = "${var.object_prefix}server"
  lifecycle {
    create_before_destroy = true
  }
}

resource "aws_security_group_rule" "server_egress" {
  type = "egress"
  from_port = 0
  to_port = 65535
  protocol = "tcp"
  cidr_blocks = ["0.0.0.0/0"]
  ipv6_cidr_blocks = ["::/0"]
  security_group_id = aws_security_group.server.id
}

resource "aws_security_group_rule" "server_ingress" {
  type = "ingress"
  from_port = 0
  to_port = 65535
  protocol = "tcp"
  cidr_blocks = ["0.0.0.0/0"]
  ipv6_cidr_blocks = ["::/0"]
  security_group_id = aws_security_group.server.id
}