locals {
  component_name = "${var.object_prefix}server"

  availability_zone_subnets = {
    "ca-central-1a" = 1
    "ca-central-1b" = 2
    "ca-central-1d" = 3
  }    
}