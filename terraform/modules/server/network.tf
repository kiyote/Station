
resource "aws_vpc" "instance" {
	cidr_block = "10.0.0.0/16"
	tags = {
		Name = local.component_name
	}
}

# Clear the default VPC security group so it has no effect
resource "aws_default_security_group" "instance" {
	tags = {
		Name = "${local.component_name} (default)"
	}
	vpc_id = aws_vpc.instance.id
}

resource "aws_internet_gateway" "instance" {
	tags = {
		Name = local.component_name
	}
	vpc_id = aws_vpc.instance.id
}

resource "aws_vpc_endpoint" "s3" {
	service_name = "com.amazonaws.${data.aws_region.current.name}.s3"
	vpc_id = aws_vpc.instance.id
}

resource "aws_subnet" "public" {
	for_each = local.availability_zone_subnets

	availability_zone = each.key
	cidr_block = cidrsubnet( aws_vpc.instance.cidr_block, 8, each.value )
	map_public_ip_on_launch = true
	tags = {
		Name = "${local.component_name} (${each.key})"
	}
	vpc_id = aws_vpc.instance.id
}

# The route table for public subnets is shared
resource "aws_route_table" "public" {
	route {
		cidr_block = "0.0.0.0/0"
		gateway_id = aws_internet_gateway.instance.id
	}
	tags = {
		Name = "${local.component_name} (public)"
	}
	vpc_id = aws_vpc.instance.id
}

data "public_subnet_ids" {
	value = [
		for key, subnet in aws_subnet.public:
		subnet.id
	]
}

resource "aws_route_table_association" "public" {
	for_each = local.availability_zone_subnets

	route_table_id = aws_route_table.public.id
	subnet_id = aws_subnet.public[ each.key ].id
}
