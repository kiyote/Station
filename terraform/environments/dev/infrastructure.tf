module "station" {
    source = "../../common"

    object_prefix = "station.dev."
}

variable "repo_token" {
  type = string
  default = ""
  sensitive = true
  nullable = false
}
