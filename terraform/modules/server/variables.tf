
data "aws_region" "current" {}

variable "object_prefix" {
  type = string
}

variable "container_publishing_role" {
  description = "The role to allow permission to push the container to the repository."
  type = string
}