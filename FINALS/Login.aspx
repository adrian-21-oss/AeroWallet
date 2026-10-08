
<!-- C:\Users\adria\source\repos\FINALS      IF YOU WANT TO LOOK FOR SQLs -->

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FINALS.Login" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link rel="stylesheet" href="~/Content/Site.css" runat="server" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
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
            width: 140%;
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



        .modal-dialog {
            width:900px;
            max-width:1000px;
            margin-left:521px;
        }
    </style>
</head>
<body>



    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div class="login-container">
            <h1 class="page-header">LOGIN</h1>

            <div class="form-group">
                <label>Username</label>
                <asp:TextBox ID="accountUsername" ValidationGroup="Login" runat="server" CssClass="input-field" placeholder="Enter your username"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_accountUsername" runat="server"
                    ControlToValidate="accountUsername"
                    ErrorMessage="Username is required"
                    ForeColor="Red"
                    ValidationGroup="Login" 
                    CssClass="error-msg"
                    Display="Dynamic">
                </asp:RequiredFieldValidator>
            </div>

            <div class="form-group">
                <label>Password</label>
                <asp:TextBox ID="accountPass" TextMode="Password" ValidationGroup="Login" runat="server" CssClass="input-field" placeholder="••••••••"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_accountPass" runat="server"
                    ControlToValidate="accountPass"
                    ErrorMessage="Password is required"
                    ForeColor="Red"
                    ValidationGroup="Login" 
                    CssClass="error-msg"
                    Display="Dynamic">
                </asp:RequiredFieldValidator>
            </div>


            <p class="text-muted" style="font-size: 13px;">Forgot Password?<a href="#" class="text-decoration-none" data-bs-toggle="modal" data-bs-target="#forgotPasswordModal"> Click here.</a></p>

            <asp:Label ID="message" runat="server" CssClass="status-label"></asp:Label>

            <asp:Button id="login" ValidationGroup="Login"  OnClick="Login_Page" Text="Log In" runat="server" CssClass="btn btn-primary"/>
            
            <asp:Button id="register" OnClick="Register_Page" CausesValidation="false" Text="Create Account" runat="server" CssClass="btn btn-secondary"/>
        </div>








        <!--MODAL-->
           <div class="modal fade"  id="forgotPasswordModal" tabindex="-1" aria-labelledby="forgotPasswordLabel" aria-hidden="true">
              <div class="modal-dialog modal-dialog-centered modal-lg">
                <div class="modal-content">
      
                  <div class="modal-header">
                    <h5 class="modal-title" id="forgotPasswordLabel">Reset Your Password</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                  </div>

                  <!-- AJAX Container to prevent full page reload -->
                  <asp:UpdatePanel ID="upForgotPassword" runat="server">
                    <ContentTemplate>
                      <div class="modal-body">
                        <p class="text-muted">Enter your registered email address below, and we'll send you a password reset link.</p>
            
                        <!-- Message Alert -->
                        <asp:Label ID="lblResetStatus" runat="server" CssClass="d-block mb-3" Visible="true"></asp:Label>

                        <!-- Input Form -->
                        <div class="mb-3" id="divEmailInput" runat="server">
                          <label for="txtResetEmail" class="form-label">Email Address</label>
                          <asp:TextBox ID="txtResetEmail" runat="server" onblur="verifyEmailApi(this.value)" ValidationGroup="ResetGroup" CssClass="form-control" TextMode="Email" placeholder="name@example.com"></asp:TextBox>
                          <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtResetEmail" 
                              ErrorMessage="Email is required." ForeColor="Red" Display="Dynamic" ValidationGroup="ResetGroup"/>
                          <asp:RegularExpressionValidator ID="revResetEmail" runat="server"
                            ControlToValidate="txtResetEmail"
                            ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                            ErrorMessage="Enter a valid email address."
                            ForeColor="Red" Display="Dynamic"
                            ValidationGroup="ResetGroup" />
                        </div>
                      </div>

                      <div class="modal-footer">
                        <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cancel</button>
            
                        <!-- Server-Side Trigger LinkButton -->
                        <asp:LinkButton ID="btnSendReset" runat="server" OnClick="btnSendReset_Click" 
                            ValidationGroup="ResetGroup" CssClass="btn btn-primary">
                            Send Reset Link
                        </asp:LinkButton>
                      </div>
                    </ContentTemplate>
                  </asp:UpdatePanel>

                </div>
              </div>
            </div>

        
    </form>

    
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>

    <script>
        async function verifyEmailApi(userEmail) {

            try {

                if (userEmail == null || userEmail.trim() == '') {
                    return;
                }

                const apiUrl = `/api/checkemailavailabilityapi?email=${encodeURIComponent(userEmail) }`;

                const response = await fetch(apiUrl, {
                    method: 'GET',
             
                });

                if (!response.ok) {
                    console.log("HTTP Error: " + response.status)
                    return;
                }

                const data = await response.json();
                const statusLabel = document.getElementById('<%= lblResetStatus.ClientID %>');


                if (statusLabel && data) {
                    statusLabel.innerText = data.message;
                    statusLabel.style.display = 'block';

                    if (data.isRegistered) {
                        statusLabel.className = 'alert alert-success d-block';
                    } else {
                        statusLabel.className = 'alert alert-danger d-block';

                    }
                }



            } catch (err) {
                console.error("Fetch execution failed: ", err);


            }




        }



    </script>
</body>
</html>




<!-- 
    
    NEEDS TO FIX
        DASHBOARD - NOTIFICATION DYMANICS and Total Money Send and Current Balance
            

        CLOUDMONEY - CHECK ACCOUNT - DONE
        
        LOGIN - FORGOT ACCOUNT - DONE
    -->