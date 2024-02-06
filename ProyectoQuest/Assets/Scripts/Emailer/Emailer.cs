using System.Collections;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Emailer : MonoBehaviour
{
    const string kSenderEmailAddress = "quest.ipst@gmail.com";
    const string kSenderPassword = "ozai gozf mdte mjvt"; //App password gmail access;
    const string kReceiverEmailAddress = "quest.ipst@gmail.com";

    void Start()
    {
        
    }

    // Method 1: Direct message
    private void SendAnEmail(string message)
    {
        // Create mail
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(kSenderEmailAddress);
        mail.To.Add(kReceiverEmailAddress);
        mail.Subject =gameObject.GetComponent<InterfaceManager>().userRut + " Formulario Quest";
        mail.Body = message;

        // Setup server 
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new NetworkCredential(
            kSenderEmailAddress, kSenderPassword) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors) {
                Debug.Log("Email success!");
                return true;
            };

        // Send mail to server, print results
        try
        {
            smtpServer.Send(mail);
        }
        catch (System.Exception e)
        {
            Debug.Log("Email error: " + e.Message);
        }
        finally
        {
            Debug.Log("Email sent!");
        }
    }

    private string WriteMessage()
    {
        InterfaceManager interfaceScript = gameObject.GetComponent<InterfaceManager>();
        string formMessage = "Nombre: " + interfaceScript.userName + "\n" +
                             "Rut: " + interfaceScript.userRut + "\n" +
                             "Email: " + interfaceScript.userEmail + "\n" +
                             "Celular: " + interfaceScript.userPhone + "\n" +
                             "Curso actual: " + interfaceScript.userGrade + "\n" +
                             "**** Áreas Santo Tomas Concepción ****" + "\n";
        foreach (Area area in interfaceScript.afinityAreas)
        {
            formMessage += area.aName + ": " + (area.affinity/4f)*100f + "%\n";
        }

        return formMessage;
    }

    public void CallSendEmail()
    {
        string message = WriteMessage();
        SendAnEmail(message);
    }
}