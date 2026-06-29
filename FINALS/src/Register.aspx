<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="FINALS.Register" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register</title>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f4f7f6;
            margin: 0;
            padding: 0;
            width:100%;
        }

        .page-header {
            margin-bottom: 30px;
            color: #2c3e50;
            font-weight: 700;
        }

        .register-container {
            width:100%;
            margin-left: auto; 
            margin-right:auto;
            margin-top:140px;

            padding: 40px; 
            max-height: 700px; 

            min-width:600px;
            max-width: 800px; 
            border-radius: 20px 14px; 
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
            box-sizing: border-box;
            background: linear-gradient(135deg, #e0fcff 0%, #ffffff 100%);
            border: 1px solid #cdecff;

        }

        h1 {
            color: #333;
            margin-top: 0;
            margin-bottom: 30px;
            letter-spacing: 2px;
        }

        .form-grid {
            display: grid;
            grid-template-columns: 200px 300px auto;
            gap: 15px;
            align-items: center;
        }

        .label-text {
            font-weight: 600;
            color: #444;
        }

        .modern-input {
            padding: 8px 12px;
            border: 1px solid #ccc;
            border-radius: 5px;
            width: 100%;
        }

        .validator-msg {
            font-size: 13px;
            margin-left: 10px;
        }

        .button-group {
            margin-top: 20px;
            padding-top: 20px;
        }

        .btn {
            padding: 10px 25px;
            border-radius: 5px;
            border: 1px solid #bbb;
            cursor: pointer;
            font-weight: bold;
            transition: background 0.3s;
            margin-right: 10px;
        }

        .btn-create {
            background-color: #f0f0f0;
        }

        .btn-create:hover {
            background-color: #e0e0e0;
        }

        .btn-back {
            background-color: transparent;
        }

        .btn-back:hover {
            background-color: rgba(0, 123, 255, 0.05);

        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="register-container">
            <h1  class="page-header">REGISTER</h1>

            <div class="form-grid">
                <span class="label-text">First Name:</span>
                <asp:TextBox ID="firstN" runat="server" CssClass="modern-input"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_firstN" ControlToValidate="firstN" ErrorMessage="First Name is required" ForeColor="Red" runat="server" CssClass="validator-msg" />

                <span class="label-text">Middle Name:</span>
                <asp:TextBox ID="middleN" runat="server" CssClass="modern-input"></asp:TextBox>
                <span></span>

                <span class="label-text">Last Name:</span>
                <asp:TextBox ID="lastN" runat="server" CssClass="modern-input"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_lastN" ControlToValidate="lastN" ErrorMessage="Last Name is required" ForeColor="Red" runat="server" CssClass="validator-msg" />

                <span class="label-text">Phone Number:</span>
                <asp:TextBox ID="phoneNum" runat="server" CssClass="modern-input" placeholder="09123456789"></asp:TextBox>
                <div>
                    <asp:RequiredFieldValidator ID="rfv_phoneNum" ControlToValidate="phoneNum" ErrorMessage="Phone Number is Required" ForeColor="Red" runat="server" CssClass="validator-msg" Display="Dynamic" />
                    <asp:RegularExpressionValidator id="rev_phoneNum" runat="server" ControlToValidate="phoneNum" ValidationExpression="^[0-9]{11}$" ErrorMessage="Must be 11 digits" ForeColor="Red" CssClass="validator-msg" Display="Dynamic" />
                </div>

                <span class="label-text">Email:</span>
                <asp:TextBox ID="email_Input" runat="server" CssClass="modern-input" placeholder="example@email.com"></asp:TextBox>
                <div>
                    <asp:RequiredFieldValidator ID="rfv_email_Input" ControlToValidate="email_Input" ErrorMessage="Email is Required" ForeColor="Red" runat="server" CssClass="validator-msg" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="rev_email_Input" ControlToValidate="email_Input" ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="Invalid Format" ForeColor="Red" runat="server" CssClass="validator-msg" Display="Dynamic" />
                </div>

                <span class="label-text">Username:</span>
                <asp:TextBox ID="accUsername_Input" runat="server" CssClass="modern-input"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_accUsername_Input" ControlToValidate="accUsername_Input" ErrorMessage="Username is required" ForeColor="Red" runat="server" CssClass="validator-msg" />

                <span class="label-text">Password:</span>
                <asp:TextBox ID="accPassword_Input" TextMode="Password" runat="server" CssClass="modern-input"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_accPassword_Input" ControlToValidate="accPassword_Input" ErrorMessage="Password is required" ForeColor="Red" runat="server" CssClass="validator-msg" />

                <span class="label-text">Confirm Password:</span>
                <asp:TextBox ID="confirm_accPassword_Input" TextMode="Password" runat="server" CssClass="modern-input"></asp:TextBox>
                <div>
                    <asp:RequiredFieldValidator ID="rfv_confirm_accPassword_Input" ControlToValidate="confirm_accPassword_Input" ErrorMessage="Please confirm password" ForeColor="Red" runat="server" CssClass="validator-msg" Display="Dynamic" />
                    <asp:CompareValidator ID="cv_accPassword_Input" runat="server" ControlToValidate="confirm_accPassword_Input" ControlToCompare="accPassword_Input" ErrorMessage="Passwords do not match" Forecolor="Red" CssClass="validator-msg" Display="Dynamic" />
                </div>
            </div>

            <div class="button-group">
                <hr style="border: 0; border-top: 1px solid #ccc; margin-bottom: 20px;" />
                <asp:Button id="createAcc" OnClick="Create_Account" Text="Create Account" runat="server" CssClass="btn btn-create"/>
                <asp:Button id="back_to_Login_Button" CausesValidation="false" OnClick="backtoLogin" Text="Back to Login" runat="server" CssClass="btn btn-back"/>
            </div>
        </div>
    </form>
</body>
</html>
