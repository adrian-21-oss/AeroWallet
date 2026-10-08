<%@ Page Title="CloudMoney" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CloudMoney.aspx.cs" Inherits="FINALS.src.CloudMoney" %>




<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .page-header {
            margin-bottom: 2rem;
            color: #2c3e50;
            font-weight: 700;
            font-size:3rem;
        }

        .transfer-card {
            background: #ffffff;
            max-width: 1400px; 
            margin: 20px 0;
            padding: 30px;
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
            border: 1px solid #cdecff;

            background: linear-gradient(135deg, #e0fcff 0%, #ffffff 100%);

        }

        .form-section {
            margin-bottom: 20px;
        }

        .input-group {
            display: flex;
            gap: 0px;
            align-items: center;
            margin-top: 8px;
        }

        .label-text {
            display: block;
            font-weight: 600;
            color: #4a5568;
            font-size: 0.95rem;
        }

        .form-input {
            padding: 10px 15px;
            border: 1.5px solid #cbd5e0;
            border-radius: 6px;
            flex-grow: 1;
            font-size: 1rem;
            transition: all 0.2s;
        }

        .form-input:hover {
             background-color: #e0e0e0;
             border-color: #999;
             transition: all 0.2s;
        }

        .form-input:focus {
            border-color: #3182ce;
            outline: none;
            box-shadow: 0 0 0 3px rgba(49, 130, 206, 0.1);
        }

        

        .btn-action { 
            padding: 10px 20px;
            border-radius: 6px;
            font-weight: 800;
            cursor: pointer;
            transition: 0.2s;
            border: none;
        }

        .btn-check {
            background-color: #edf2f7;
            color: #2d3748;
            top: 0px;
            left: 250px;
        }

        .btn-check:hover {
            background-color: #e2e8f0;
        }

        .btn-send {
            background-color: #3182ce;
            color: white;
            width: 100%;
            margin-top: 15px;
            font-size: 1.1rem;
        }

        .btn-send:hover {
            background-color: #2b6cb0;
        }

        .display-info {
            background: #f7fafc;
            padding: 12px;
            border-radius: 6px;
            margin: 10px 0;
            border-left: 4px solid #3182ce;
            font-size: 0.9rem;
        }

        .error-small {
            font-size: 0.8rem;
            display: block;
            margin-top: 4px;
        }





        .transfer-disclaimer {
            
            font-size:12px;

            color: #718096;

            margin-top:40px;
            margin-bottom:10px;
            max-width:320px;
        }





        .acc-check {
            padding: 10px 25px;
            background-color: #f0f0f0;
            border: 1px solid #bbb;
            border-radius: 6px;
            cursor: pointer;
            font-weight: bold;
            transition: all 0.2s;
            margin-top: 0px;
            margin-left: 0px;

        }

        .acc-check:hover {
             background-color: #e0e0e0;
             border-color: #999;
        }

    </style>

    <main>
        <br />
        <br />

        <h1 class="page-header">CLOUDMONEY</h1>
        <hr style="border: 0; border-top: 1px solid #ccc; margin-bottom: 20px;"  />

        <div class="transfer-card">
            





            <div class="form-section">
                <label class="label-text">Enter Account No.</label>
                <div class="input-group">
                    <asp:TextBox ID="accountNumber_Input" runat="server" onblur="verifyAccountApi(this.value)" Visible="true" CssClass="form-input" placeholder="e.g. 1234567"></asp:TextBox>
                    <!--<asp:Button id="checkID" OnClick="Check_ID" CssClass="acc-check" CausesValidation="false" runat="server" Text="Check Account"/>   -->
                </div>
                <asp:RequiredFieldValidator ID="rfv_accountNumber_Input" runat="server"
                    ControlToValidate="accountNumber_Input" ErrorMessage="Account number is required"
                    ForeColor="Red" CssClass="error-small" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:Label ID="message" runat="server" CssClass="error-small"></asp:Label>
            </div>



            <div class="display-info">
                <asp:Label ID="AccountIDDisplay" runat="server" Text="Account Number: ---" style="display:block; font-weight:bold;"></asp:Label>
                <asp:Label ID="NameDisplay" runat="server" Text="Recipient Name: ---"></asp:Label>
            </div>



            <br />
            <br />

            <div class="form-section">
                <label class="label-text">Amount to Send</label>
                <asp:TextBox id="sendMoney_Input" runat="server" CssClass="form-input" placeholder="0" style="width:100%; box-sizing:border-box;"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_sendMoney_Input" runat="server"
                    ControlToValidate="sendMoney_Input" 
                    ErrorMessage="Amount to send is required"
                    ForeColor="Red" 
                    CssClass="error-small" 
                    Display="Dynamic"></asp:RequiredFieldValidator>
                
                <asp:RangeValidator ID="rv_sendMoney_Input" runat="server"
                    ControlToValidate="sendMoney_Input"
                    MaximumValue="2000"
                    MinimumValue="100"
                    Type="Integer"
                    ErrorMessage="Must be minimum of 100 and maximum of 2000"
                    Forecolor="Red"
                    CssClass="error-text"
                    Display="Dynamic">
                </asp:RangeValidator>



            </div>

            <div class="form-section">
                <label class="label-text">Verify Sender Password</label>
                <asp:TextBox ID="accountPass" TextMode="Password" runat="server" CssClass="form-input" placeholder="••••••••" style="width:100%; box-sizing:border-box;"></asp:TextBox>
                <asp:RequiredFieldValidator 
                    ID="rfv_accountPass" runat="server"
                    ControlToValidate="accountPass" 
                    ErrorMessage="Account password is required"
                    ForeColor="Red" 
                    CssClass="error-small" 
                    Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
             
            <p class="transfer-disclaimer">
                Please ensure the recipient details and amount are correct. Transactions cannot be undone once processed.
            </p>

            <asp:Button id="btn_sendMoney" text="Send Money" OnClick="SendMoney" runat="server" CssClass="btn-action btn-send"/>

            <div style="text-align: left; margin-top: 10px;">
                <asp:Label ID="message2" runat="server" ForeColor="red" CssClass="error-small"></asp:Label>
            </div>

        </div>

        <script>
            async function verifyAccountApi(accountId) {
                try {

                    if (!accountId || !accountId.trim() || accountId.trim() == '') {
                        return;
                    }


                    const apiUrl = `/api/checkaccountavailabilityapi?accountId=${encodeURIComponent(accountId)}`;

                    const response = await fetch(apiUrl, {
                        method: 'GET'
                    })

                    if (!response.ok) {
                        console.log("HTTP Error: " + response.status)
                        return;
                    }



                    const data = await response.json();
                    const accountIDDisplay = document.getElementById('<%= AccountIDDisplay.ClientID %>');
                    const accountNameDisplay = document.getElementById('<%= NameDisplay.ClientID %>');

                    if (!accountIDDisplay && !accountNameDisplay) {
                        return;
                    }


                    if (data) {

                        accountIDDisplay.innerText = `Account Number: ${data.accountId}`;
                        accountNameDisplay.innerText = `Recipient Name: ${data.accountFullName}`;

                    }


                } catch (err) {

                    console.error("HTTP Error: ", err)

                }
                



            }
        </script>
    </main>
</asp:Content>