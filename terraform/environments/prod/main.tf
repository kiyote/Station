# For deploying AWS resources
provider "aws" {
  region = var.aws_region

  assume_role {
    role_arn = var.aws_deployment_role
  }
}

# For managing the terraform state
terraform {
  backend "s3" {
    dynamodb_table = var.aws_state_locktable
    bucket = var.aws_state_bucket
    key    = var.aws_state_key
    region = var.aws_region
    role_arn = var.aws_state_role
  }
}

module "webclient" {
    source = "../modules/webclient"

    object_prefix = var.object_prefix
    repo_token = var.repo_token
    webclient_stage = var.webclient_stage
    source_repository = var.source_repository
    source_repository_branch = var.source_repository_branch
}
