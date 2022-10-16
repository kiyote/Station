variable "repo_token" {
  description = "Carries the access token for the repository. This value comes from TF_VAR_repo_token in the GitHub Environment."
  type = string
  default = ""
  sensitive = true
  nullable = false
}

variable "object_prefix" {
  description = "A value to add as a prefix to all deployed resources in order to disambiguate stage (prod vs dev)."
  type = string  
}

variable "aws_region" {
  description = "The region these resources are being deployed to."
  type = string
}

variable "aws_deployment_role" {
  description = "The role to assume in order to deploy AWS resources."
  type = string
}

variable "source_repository" {
  description = "The git repository containing the webclient source."
  type = string
}

variable "source_repository_branch" {
  description = "The branch within the repository containing the source to deploy."
  type = string
}

variable "webclient_stage" {
  description = "The deployment stage for the webclient application.  DEVELOPMENT or PRODUCTION"
  type = string
}