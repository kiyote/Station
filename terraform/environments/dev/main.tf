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
  region = var.aws_region

  assume_role {
    role_arn = var.aws_deployment_role
  }
}

# Resources that make up the application below
# ********************************************
module "webclient" {
    source = "../../modules/webclient"

    object_prefix = var.object_prefix
}
