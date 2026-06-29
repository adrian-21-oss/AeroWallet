


<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FINALS.Login" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
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

        .login-container {
            background: linear-gradient(135deg, #e0fcff 0%, #ffffff 100%);
            margin-left:-35px;
            width: 100%;
            max-width: 400px; 
            padding: 40px;
            border-radius: 12px;
            box-shadow: 0 10px 25px rgba(0, 0, 0, 0.05);
            border: 1px solid #e1e4e8;

        }

        .page-header {
            margin-bottom: 30px;
            color: #2c3e50;
            font-weight: 700;
        }

        h1 {
            margin: 0 0 20px 0;
            font-size: 24px;
            color: #333;
            text-align: center;
            letter-spacing: 1px;
        }

        .form-group {
            margin-bottom: 20px;
        }

        label {
            display: block;
            font-size: 14px;
            font-weight: 600;
            margin-bottom: 8px;
            color: #555;
        }

        .input-field {
            width: 100%;
            padding: 12px;
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

        .btn {
            width: 100%;
            padding: 12px;
            border: none;
            border-radius: 6px;
            font-size: 16px;
            font-weight: 600;
            cursor: pointer;
            transition: background 0.3s;
        }

        .btn-primary {
            background-color: #007bff;
            color: white;
            margin-bottom: 10px;
        }

        .btn-primary:hover {
            background-color: #0056b3;
        }

        .btn-secondary {
            background-color: transparent;
            color: #007bff;
            border: 1px solid #007bff;
        }

        .btn-secondary:hover {
            background-color: rgba(0, 123, 255, 0.05);
        }

        .status-label {
            text-align: center;
            display: block;
            margin-bottom: 15px;
            font-size: 14px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <h1 class="page-header">LOGIN</h1>

            <div class="form-group">
                <label>Username</label>
                <asp:TextBox ID="accountUsername" runat="server" CssClass="input-field" placeholder="Enter your username"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_accountUsername" runat="server"
                    ControlToValidate="accountUsername"
                    ErrorMessage="Username is required"
                    ForeColor="Red"
                    CssClass="error-msg"
                    Display="Dynamic">
                </asp:RequiredFieldValidator>
            </div>

            <div class="form-group">
                <label>Password</label>
                <asp:TextBox ID="accountPass" TextMode="Password" runat="server" CssClass="input-field" placeholder="••••••••"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_accountPass" runat="server"
                    ControlToValidate="accountPass"
                    ErrorMessage="Password is required"
                    ForeColor="Red"
                    CssClass="error-msg"
                    Display="Dynamic">
                </asp:RequiredFieldValidator>
            </div>

            <asp:Label ID="message" runat="server" CssClass="status-label"></asp:Label>

            <asp:Button id="login" OnClick="Login_Page" Text="Log In" runat="server" CssClass="btn btn-primary"/>
            
            <asp:Button id="register" OnClick="Register_Page" CausesValidation="false" Text="Create Account" runat="server" CssClass="btn btn-secondary"/>
        </div>
    </form>
</body>
</html>
