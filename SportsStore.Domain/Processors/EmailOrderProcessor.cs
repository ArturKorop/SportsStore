using System.Net;
using System.Net.Mail;
using System.Text;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Interfaces;

namespace SportsStore.Domain.Processors
{
    public class EmailSettings
    {
        public string MailToAddress = "orders@example.com";
        public string MailFromAddress = "sportsstore@example.com";
        public bool UseSsl = true;
        public string Username = "MySmtpUsername";
        public string Password = "MySmtpPassword";
        public string ServerName = "smtp.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = true;
        public string FileLocation = @"D:\Archive";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private readonly EmailSettings _settings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            _settings = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            using(var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = _settings.UseSsl;
                smtpClient.Host = _settings.ServerName;
                smtpClient.Port = _settings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_settings.Username, _settings.Password);

                if(_settings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = _settings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                var sb = new StringBuilder();
                sb.AppendLine("A new order has been submitted");
                sb.AppendLine("---");
                sb.AppendLine("Items:");

                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.Price*line.Quantity;
                    sb.AppendFormat($"{line.Quantity} x {line.Product.Name} subtotal: {subtotal:c}");
                }

                sb.AppendFormat($"Total order value: {cart.ComputeTotalValue():c}");
                sb.AppendLine("---");
                sb.AppendLine("Ship to:");
                sb.AppendLine(shippingDetails.Name);
                sb.AppendLine(shippingDetails.Line1);
                sb.AppendLine(shippingDetails.Line2 ?? "");
                sb.AppendLine(shippingDetails.Line3 ?? "");
                sb.AppendLine(shippingDetails.City);
                sb.AppendLine(shippingDetails.State ?? "");
                sb.AppendLine(shippingDetails.Country);
                sb.AppendLine(shippingDetails.Zip);
                sb.AppendLine("---");
                sb.AppendLine($"Girft warp: {(shippingDetails.GiftWrap ? "Yes" : "No")}");

                var mailMessage = new MailMessage(_settings.MailFromAddress, _settings.MailToAddress,
                    "New order submitted", sb.ToString());

                smtpClient.Send(mailMessage);
            }
        }
    }
}