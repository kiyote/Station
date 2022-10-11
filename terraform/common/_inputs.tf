variable "object_prefix" {
  type = string
  validation {
    condition = contains(["station.dev.", "station.prod."], var.object_prefix)
    error_message = "The object prefix must be set to `station.dev.` or `station.prod.`."
  }
}

variable "repo_token" {
  type = string
}

variable "amplify_stage" {
  type = string
  nullable = false
  default = "DEVELOPMENT"
}