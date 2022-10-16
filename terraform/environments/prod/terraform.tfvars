# Infrastructure vars
aws_region = "ca-central-1"
aws_deployment_role = "arn:aws:iam::860568434255:role/station_deploy_prod"
aws_state_bucket = "kiyote.terraformstate"
aws_state_locktable = "kiyote.terraformstate"
aws_state_key = "prod/station"
aws_state_role = "arn:aws:iam::860568434255:role/terraform_state_prod"

# Webclient vars
object_prefix = "station.prod."
webclient_stage = "PRODUCTION"
source_repository = "https://github.com/kiyote/Station"
source_repository_branch = "main"