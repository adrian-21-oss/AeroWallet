<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="FINALS._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        body {
            background-color: #f0f4f8;
            font-family: 'Segoe UI', Roboto, Helvetica, Arial, sans-serif;
        }

        .main-container {
            padding: 20px;
        }

        

        .header-title {
            font-size: 3rem;
            font-weight: 700;
            color: #2d3748;
            margin-bottom: 10px;
            display: block;
        }

        .wallet-card {
            padding: 30px;
            background: linear-gradient(135deg, #e0fcff 0%, #ffffff 100%);
            height: 600px;
            max-width: 1281px;
            border-radius: 16px;
            box-shadow: 0 10px 25px rgba(0,0,0,0.05);
            border: 1px solid #cdecff;
            position: relative;
            background-color: lightcyan;

        }

        .info-label {
            display: block;
            margin-bottom: 25px;
            font-size: 18px;
            color: #4a5568;
            transition: transform 0.2s;
        }

        .info-label span {
            font-weight: 600;
            color: #2b6cb0;
        }

        #notification {

            position: relative;
            background-color: #ffffff;

            min-height: 379px;
            max-height:479px;
            max-width: 396px;

            border-radius: 16px;
            box-shadow: 0 15px 35px rgba(0,0,0,0.1);
            margin-top: -260px;
            margin-left: auto;
            margin-bottom: 20px;
            margin-right:15px;

            border: 1px solid #e2e8f0;
            overflow-y: auto;
        }

        .notification-header {
            padding: 15px 20px;
            background: #f8fafc;
            border-bottom: 1px solid #edf2f7;
            font-weight: 700;
            color: #1a202c;
            border-radius: 16px 16px 0 0;
        }

        .notification-item {
            border-bottom: 1px solid #f1f5f9;
            padding: 15px 20px;
            font-size: 14px;
            line-height: 1.5;
            color: #4a5568;
            transition: background 0.2s;
        }

        .notification-item:hover {
            background-color: #f7fafc;
        }

        .notification-item strong {
            color: #2d3748;
        }

        .amount-highlight {
            color: #38a169;
            font-weight: 700;
        }

        .no-data-message {
            padding: 40px 20px;
            text-align: center;
            color: #a0aec0;
        }
    </style>

    <main class="main-container">
        <br />
        <asp:Label id="Header" runat="server" CssClass="header-title">Your Name's Wallet</asp:Label>
        <hr style="border: 0; border-top: 1px solid #d1d5db; margin-bottom: 20px;" />
        
        <div>
            <div class="wallet-card">
                <div class="info-label">Account No: <asp:Label id="user_accountNo" runat="server" font-bold="true"></asp:Label></div>
                <div class="info-label">Full Name: <asp:Label id="user_fullName" runat="server" font-bold="true"></asp:Label></div>
                <div class="info-label">Joined: <asp:Label id="user_dateRegistered" runat="server" font-bold="true"></asp:Label></div>
                <div class="info-label">Current Balance: <asp:Label id="user_totalCurrentBalance" runat="server" font-bold="true" ForeColor="#38a169"></asp:Label></div>
                <div class="info-label">Total Money Sent: <asp:Label id="user_totalAmountSent" runat="server" font-bold="true" ForeColor="#e53e3e"></asp:Label></div>
                
                <div id="notification">
                    <div class="notification-header">Recent Activity</div>
                    <asp:Repeater ID="RPTNotification" runat="server">
                        <ItemTemplate>
                            <div class="notification-item">
                                <strong><%# Eval("Full_Name") %></strong> sent you 
                                <span class="amount-highlight"><%# Eval("Amount") %> pesos</span> 
                                via CloudMoney
                                <div style="font-size: 11px; color: #a0aec0; margin-top: 4px;">
                                    <%# Eval("Transaction_Date", "{0:yyyy-MM-dd}") %> at <%# Eval("FormattedTransaction_Time") %>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
    
                    <asp:Panel ID="pnlNoData" runat="server" Visible="false">
                        <div class="no-data-message">
                            <p>No transactions found.<br/>Start sending money with CloudMoney!</p>
                        </div>
                    </asp:Panel>
                </div>
            
            </div>

            
        </div>
    </main>
</asp:Content>