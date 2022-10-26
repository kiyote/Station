
locals {
  s3_origin_id = "webclient_s3_origin"
}

resource "aws_cloudfront_origin_access_control" "distribution_access" {
    origin_access_control_origin_type = "s3"
    signing_behavior = "always"
    signing_protocol = "sigv4"
}

resource "aws_cloudfront_distribution" "distribution_webclient" {
  origin {
    domain_name = aws_s3_bucket.bucket_webclient.bucket_regional_domain_name
    origin_access_control_id = aws_cloudfront_origin_access_control.distribution_access.id
    origin_id = locals.s3_origin_id
  }

  enabled = true
  is_ipv6_enabled = true
  default_root_object = "index.html"
  price_class = "PriceClass_100"

  restrictions {
    geo_restriction {
      restriction_type = "whitelist"
      locations        = ["US", "CA"]
    }
  }

  viewer_certificate {
    cloudfront_default_certificate = true
  }  
}