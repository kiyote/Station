module "shared" {
	source = "../modules/amplify"

	object_prefix = var.object_prefix
}