module "station" {
    source = "../../common"

    object_prefix = "station.dev."
    repo_token = var.repo_token
}

variable "repo_token" {
  description = "Carries the access token for the repository."
  type = string
  default = ""
  sensitive = true
  nullable = false
}
