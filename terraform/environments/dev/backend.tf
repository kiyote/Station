# For deploying AWS resources
provider "aws" {
  region = "ca-central-1"

  assume_role {
    role_arn = "arn:aws:iam::860568434255:role/station_deploy_dev"
  }
}

# For managing the terraform state
terraform {
  backend "s3" {
    bucket = "kiyote.terraformstate"
    dynamodb_table = "kiyote.terraformstate"
    key    = "dev/station"
    region = "ca-central-1"
    role_arn = "arn:aws:iam::860568434255:role/terraform_state_dev"
  }
}