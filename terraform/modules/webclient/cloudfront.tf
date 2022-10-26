
locals {
  s3_origin_id = "webclient_s3_origin"
}

resource "aws_cloudfront_distribution" "distribution_webclient" {
  origin {
    domain_name = aws_s3_bucket.bucket_webclient.bucket_regional_domain_name
    origin_access_control_id = aws_cloudfront_origin_access_control.default.id
    origin_id = locals.s3_origin_id
  }

  enabled = true
  is_ipv6_enabled = true
  default_root_object = "index.html"

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