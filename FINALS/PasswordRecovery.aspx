<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PasswordRecovery.aspx.cs" Inherits="FINALS.PasswordRecovery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PasswordRecovery</title>

    <style>
        body {
            margin: 0;
            padding: 0;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f4f7f6;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }


        .main-content {

            width: 600px;
            height: 800px;

            max-height: 60vh;

            max-width: 600px;
            min-width: auto;

            background: linear-gradient(135deg, #e0fcff 0%, #ffffff 100%);
            box-shadow: 0 10px 25px rgba(0, 0, 0, 0.05);
            border-radius: 12px;
            border: 1px solid #e1e4e8;

            text-align:center;

            display: flex;
            flex-direction:column;
            align-items: center;
            gap: 15px;
        }


        .page-header {
            margin-bottom: 10px;
            color: #2c3e50;
            font-weight: 700;

            margin-top:50px;
        }


        .page-body div {
            display: flex;
            flex-direction: column;
            align-items: center;
        }

        
     
        .btn-primary {
            background-color: #007bff;
            color: white;
            margin-bottom: 10px;

            left: 50px;
            margin-top: 20px;
            padding: 10px 20px; 
            font-size: 16px;
            border: none;
            border-radius: 5px;
            cursor: pointer;

            text-align: center;
            align-content: center;

            transition: all 0.3s ease;
        }

        .btn-primary:hover {

            background-color: #0056b3;

        }

        .display-account {
            display: flex;
            flex-direction:column;
            align-items:center;
            gaps:5px;

            margin-bottom: 50px;
        }

        
        .form-group {
            margin-bottom: 20px;
        }
        
        

        .input-field {
            width: 150%;
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 6px;

            box-sizing: border-box;
            transition: border-color 0.3s ease;

        }

        .input-field:focus {
            border-color: #007bff;
            outline: none;
            box-shadow: 0 0 0 3px rgba(0, 123, 255, 0.1);
        }


        .error-msg {
            font-size: 12px;
            display: block;
            margin-top: 5px;
        }

        label {
            display: block;
            font-size: 14px;
            font-weight: 600;
            margin-bottom: 8px;
            color: #555;
        }

        
        h1 {
            margin: 0 0 20px 0;
            font-size: 26px;
            color: #333;
            text-align: center;
            letter-spacing: 1px;
            
        }

        h5 {
            margin: 0 0 20px 0;
            font-size: 18px;
            color: #333;
            text-align: center;
            letter-spacing: 0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main-content">

            <h1 class="page-header">ACCOUNT RECOVERY</h1>

            <div class="page-body">




                <div class="display-account">
                    <h5>Account Information</h5>
                    <asp:Label id="accountIDDisplay" runat="server">Account ID: xxx</asp:Label>
                    <asp:Label id="fullNameDisplay" runat="server">Full Name: xxx</asp:Label>
                    <asp:Label id="usernameDisplay" runat="server">Account Username: xxx</asp:Label>
                </div>

                <div class="newPass">
                    <label>New Password</label>
                    <asp:TextBox ID="newPasswordInput" TextMode="Password" ValidationGroup="Recover" runat="server" CssClass="input-field" placeholder="••••••••"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfv_newPasswordInput" runat="server"
                        ControlToValidate="newPasswordInput"
                        ErrorMessage="New password is required"
                        ForeColor="Red"
                        ValidationGroup="Recover" 
                        CssClass="error-msg"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>

                <br />
                

                <div class="confPass">
                    <label>Confirm Password</label>
                    <asp:TextBox ID="confirmPass" TextMode="Password" ValidationGroup="Recover" runat="server" CssClass="input-field" placeholder="••••••••"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfv_confirmPass" runat="server"
                        ControlToValidate="confirmPass"
                        ErrorMessage="Confirm password is required"
                        ForeColor="Red"
                        ValidationGroup="Recover" 
                        CssClass="error-msg"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>

                <asp:CompareValidator ID="cv_password" runat="server"
                    ControlToCompare="newPasswordInput"
                    ControlToValidate="confirmPass"
                    ErrorMessage="Passwords do not match"
                    ForeColor="Red"
                    ValidationGroup="Recover"
                    CssClass="error-msg"
                    Display="Dynamic">
                </asp:CompareValidator>



            </div>




            <asp:Button id="btn_recoverPasswordId" OnClick="RecoverChangePassword" CssClass="btn btn-primary" ValidationGroup="Recover" Text="Change Password" runat="server"/>

        </div>
    </form>
</body>
</html>
