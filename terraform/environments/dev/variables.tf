
variable "aws_region" {
  description = "The region these resources are being deployed to."
  type = string
}

variable "aws_deployment_role" {
  description = "The role to assume in order to deploy AWS resources."
  type = string
}

variable "github_action_role" {
  description = "The role that github actions are performed under."
  type = string
}

variable "object_prefix" {
  description = "A value to add as a prefix to all deployed resources in order to disambiguate stage (prod vs dev)."
  type = string  
}
