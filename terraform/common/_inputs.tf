variable "object_prefix" {
  type = string
  validation {
    condition = contains(["station.dev.", "station.prod."], var.object_prefix)
    error_message = "The object prefix must be set to `station.dev.` or `station.prod.`."
  }
}
