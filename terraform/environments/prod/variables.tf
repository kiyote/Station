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

variable "aws_state_bucket" {
  description = "The name of the bucket that the deployment state for this project is to be stored in."
  type = string
}

variable "aws_state_locktable" {
  description = "The name of the dynamodb table used to hold the state lock."
  type = string
}

variable "aws_state_key" {
  description = "The S3 key where the state will be written."
  type = string
}

variable "aws_state_role" {
  description = "The role used to read and write the terraform state in S3."
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