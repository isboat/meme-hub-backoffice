﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meme.Domain.Models;
using MemeTokenHub.Backoffce.Models;

namespace MemeTokenHub.Backoffce.Services
{
    public static class EmailTemplates
    {
        public static string GetTenantCreatedEmailBody(MemePageModel model)
        {
            var builder = new StringBuilder();
            builder.Append($"<p>Dear {model.Name},</p>");
            builder.Append($"<p>Welcome to onScreenSync TV Screen Management service! Your platform is now created and opened for use. We're thrilled to have you on board and we're excited to see what you'll accomplish with our platform. Here are a few things you can do now that you're part of our community:</p>");
            builder.Append("<ul>");
            builder.Append($"<li><strong>Explore Our Features</strong>: Take some time to navigate through our platform and discover all the tools and features we offer to help you. Visit <a href='http://myscreensyncservice.runasp.net/'>Management Dashboard to get started.</a></li>");
            builder.Append("<li><strong>Add users as content editors</strong>: From the management dashboard, you can add users who can add and updated engaging content for your audience.</li>");
            builder.Append("</ul>");
            builder.Append("<p>If you have any questions or need assistance, don't hesitate to reach out to our support team at support@onscreensync.com or visit our Help Center for <a href='https://onscreensync.com/faq.html'>FAQs and troubleshooting guides</a>.</p>");
            builder.Append("<p>Once again, welcome to onScreenSync TV Screen Management service! We're committed to providing you with a seamless and rewarding experience, and we look forward to helping you achieve your goals.</p>");
            builder.Append("<p>Best regards,<br />onScreenSync.com<p/>");
            return builder.ToString();
        }

        public static string GetTenantGdprEmailBody(MemePageModel model)
        {
            var builder = new StringBuilder();
            builder.Append($"<p>Dear {model.Name},</p>");
            builder.Append($"<p>We value your trust and are committed to protecting your privacy and personal data. As part of our ongoing efforts to ensure compliance with the General Data Protection Regulation (GDPR), we are updating our Privacy Policy and terms of service.</p>");
            builder.Append("<h2>How We Use Your Information:</h2>");
            builder.Append("<p>We collect and process your personal information for the following purposes:</p>");
            builder.Append("<ul>");
            builder.Append($"<li>To provide and maintain our services to you.</li>");
            builder.Append($"<li>To communicate with you, including responding to your inquiries and providing customer support.</li>");
            builder.Append($"<li>To personalize and improve your experience with our services.</li>");
            builder.Append($"<li>To analyze how our services are used and improve their functionality.</li>");
            builder.Append($"<li>To comply with legal and regulatory requirements.</li>");
            builder.Append($"<li>To analyze how our services are used and improve their functionality.</li>");
            builder.Append($"<li>To analyze how our services are used and improve their functionality.</li>");
            builder.Append("</ul>");
            builder.Append("<p>You have the right to access, correct, or delete your personal information at any time and if you have any questions or concerns about your privacy or data protection, please contact us via support@onscreensync.com</p>");
            builder.Append("<p>Best regards,<br />onScreenSync.com<p/>");
            return builder.ToString();
        }
    }
}
