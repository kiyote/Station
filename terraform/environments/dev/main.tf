# For managing the terraform state
# Variables cannot be used in the `terraform` block
terraform {
  backend "s3" {
    dynamodb_table = "kiyote.terraformstate"
    bucket = "kiyote.terraformstate"
    key    = "dev/station"
    region = "ca-central-1"
    role_arn = "arn:aws:iam::860568434255:role/station_deploy_dev"
  }
}

# For deploying AWS resources
provider "aws" {
  region = "ca-central-1"

  assume_role {
    role_arn = var.aws_deployment_role
  }
}

# Resources that make up the application below
# ********************************************
module "webclient" {
  source = "../../modules/webclient"

  object_prefix = var.object_prefix
  bucket_copy_role = var.github_action_role
  cloudfront_prefix = var.cloudfront_prefix
}

module "server" {
  source = "../../modules/server"

  object_prefix = var.object_prefix
  service_prefix = var.service_prefix
  container_publishing_role = var.github_action_role
  image_prefix = var.image_prefix
  server_image = "e3f4d3cbdb596378efbd02d6bc2164065f05446d-build46"
}
