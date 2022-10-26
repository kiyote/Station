variable "object_prefix" {
  type = string
}

variable "aws_deployment_role" {
  description = "The role to assume in order to deploy AWS resources."
  type = string
}