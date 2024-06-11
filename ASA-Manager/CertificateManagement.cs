using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ARKServerCreationTool
{
    internal static class CertificateManagement
    {
        public static Dictionary<string, string> certsToInstall = new Dictionary<string, string>
        {
            {
                "Amazon Root CA 1", "https://www.amazontrust.com/repository/AmazonRootCA1.cer"
            },
            {
                "Amazon RSA 2048 M02", "https://crt.r2m02.amazontrust.com/r2m02.cer"
            }
        };

        public static bool CheckCertificateIsInstalled(object findValue)
        {
            X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            var certificates = store.Certificates.Find(
                X509FindType.FindBySubjectName,
                findValue: findValue,
                false);

            return (certificates != null && certificates.Count > 0);
        }

        public static void InstallCertificate(X509Certificate2 cert)
        {
            using (X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadWrite);
                store.Add(cert); //where cert is an X509Certificate object
            }
        }

        public static void InstallCertificateFromURL(string url)
        {
            using (WebClient wc = new WebClient())
            {
                InstallCertificate(new X509Certificate2(wc.DownloadData(url)));
            }
        }
        
        public static IEnumerable<string> GetMissingCertificates()
        {
            return certsToInstall.Keys.Where(k => !CheckCertificateIsInstalled(k));
        }

        public static void InstallMissingCertificates()
        {
            var missingCerts = GetMissingCertificates();

            foreach (string key in missingCerts)
            {
                InstallCertificateFromURL(certsToInstall[key]);
            }
        }       
        
        public static void InstallCertByName(string name)
        {
            InstallCertificateFromURL(certsToInstall[name]);
        }
    }
}