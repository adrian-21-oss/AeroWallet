<%@ Page Title="Report" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="FINALS.Report" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
       
        .report-main {
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 0px; 
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        .page-header {
            margin-bottom: 2rem;
            color: #2c3e50;
            font-size:3rem;
            font-weight: 700;
        }

        .report-section {
            background: linear-gradient(135deg, #e0fcff 0%, #ffffff 100%);


            width: 100%;

            max-width: 1300px; 
            min-width: 500px;

            max-height:800px;

            margin: 20px 0;
            padding: 30px;

            border-radius: 20px 14px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
            display: flex;
            flex-direction: column;
            align-items: center;
            box-sizing: border-box;
        }

        .report-section h1 {
            color: #2c3e50;
            margin-bottom: 20px;
        }

        .filter-group {
            display: flex;
            flex-direction: column;
            align-items: center;
            margin-bottom: 15px;
            width: 100%;
        }



        .modern-input {
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 4px;
            margin-top: 5px;
            text-align: center;
        }
        .modern-input-list {
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 4px;
            margin-top: 5px;
            text-align: center;
            background-color: #76F00C;
        }
        
        
        .modern-input-list:hover {
             background-color: #52D017;
             border-color: #999;
             transition: all 0.2s;
        }


        .modern-input:hover {
             background-color: #e0e0e0;
             border-color: #999;
             transition: all 0.2s;
        }



        .div_gridContainer {
            width: 100%;
            max-width: 1000px;
            height: 340px;
            overflow: auto;
            border: 1px solid #ccc;
            background-color: white;
            margin-top: 20px;
        }

        .my-grid {
            border-collapse: collapse;
            width: 100%;
        }

        .my-grid th {
            position: sticky;
            top: 0;
            background-color: #ececec;
            z-index: 10;
            padding: 10px;
            box-shadow: 0 2px 2px -1px rgba(0,0,0,0.4);
        }

        .my-grid td {
            padding: 8px;
            border-bottom: 1px solid #eee;
            text-align: center;
        }

        .button-row {
            margin-top: 15px;
        }









        .div_gridContainer {
            background-color: white;
            border: 1px solid #e0e0e0;
            border-radius: 4px;
            width: 100%; 
            margin-top: 20px;
            max-height: 400px;
            overflow-y: auto;
        }

        .div_gridContainer::-webkit-scrollbar {      /*Scroll bar Size*/
            width: 8px;
            height: 8px;
        }

        .div_gridContainer::-webkit-scrollbar-thumb {    /*Color*/
            background-color: #cbd5e1;
            border-radius: 10px;
        }

        .div_gridContainer::-webkit-scrollbar-thumb:hover {
            background-color: #94a3b8;
        }




        .my-grid {
            width: 100%; /* Ensures the table elements stretch to fill the white container */
            border-collapse: collapse;
        }
        

        .empty-grid-message {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            font-size: 15px;
            letter-spacing: 0.5px;
        }


 
    </style>

    <main class="report-main">
        <br />
        <br />
        <div style="width: 100%; max-width: 1300px;">
            <h1 class="page-header">HISTORY</h1>
            <hr style="border: 0; border-top: 1px solid #ccc; margin-bottom: 20px;"  />
        </div>

        <!-- Section 1: Statement of Account -->
        <br />
        <section class="report-section">
            
            <h1>Statement of Account</h1>
            
            <div class="filter-group">
                <label>From:</label>
                <asp:TextBox ID="dateFrom" placeholder="MM/DD/YYYY" runat="server" TextMode="Date" CssClass="modern-input" Width="320px"></asp:TextBox>
                <asp:CompareValidator ID="cv_dateFrom" runat="server" ControlToValidate="dateFrom" Type="Date" Operator="DataTypeCheck" ErrorMessage="Invalid Date" ForeColor="Red" Display="Dynamic" />
            </div>

            <div class="filter-group">
                <label>To:</label>
                <asp:TextBox ID="dateTo" placeholder="MM/DD/YYYY" runat="server" TextMode="Date" CssClass="modern-input" Width="320px"></asp:TextBox>
                <asp:CompareValidator ID="cv_dateTo" runat="server" ControlToValidate="dateTo" Type="Date" Operator="DataTypeCheck" ErrorMessage="Invalid Date" ForeColor="Red" Display="Dynamic" />
                <asp:Label ID="message2" ForeColor="red" runat="server"></asp:Label>
            </div>

            <div class="button-row">
                <asp:Button ID="listItem" CssClass="modern-input-list" OnClick="DisplayTable" runat="server" Text="View List" Width="100px" />
                <asp:Button ID="viewDashboard" CssClass="modern-input" runat="server" Text="View Dashboard" OnClick="BackToDashboard" Width="130px" />
            </div>

            <!-- Section 1: Statement of Account -->
            <div class="div_gridContainer">
            
                <!-- my_grid, empty-grid-message -->
                <asp:GridView ID="grdVwAllTransactions" CssClass="my-grid" runat="server" EmptyDataText="Select a date range and click 'List' to generate your statement." AutoGenerateColumns="false">
                    <EmptyDataRowStyle CssClass="empty-grid-message" Height="337px" />
                    <Columns>
                        <asp:BoundField DataField="SEQNO" HeaderText="Seq. #" />
                        <asp:BoundField DataField="TYPE" HeaderText="Type" />
                        <asp:BoundField DataField="DATE" HeaderText="Date"/>
                        <asp:BoundField DataField="DEBIT" HeaderText="Debit"/>
                        <asp:BoundField DataField="CREDIT" HeaderText="Credit"/>
                        <asp:BoundField DataField="BALANCE" HeaderText="Balance"/>
                        <asp:BoundField DataField="SENDTO" HeaderText="Sent To" />
                        <asp:BoundField DataField="RECEIVEFROM" HeaderText="Receive From" />
                    </Columns>
                </asp:GridView>
            </div>
        </section>
        <br />
        <br />
        <br />
        <br />        
        <br />
        <br />



        <!-- Section 2: Deposits and Withdrawals -->
        <section class="report-section">
            <h1>My Deposits and Withdrawals</h1>
            
            <div class="filter-group">
                <label>From:</label>
                <asp:TextBox ID="dateFrom2" placeholder="MM/DD/YYYY" runat="server" TextMode="Date" CssClass="modern-input" Width="320px"></asp:TextBox>
                <asp:CompareValidator ID="cv_dateFrom2" runat="server" ControlToValidate="dateFrom2" Type="Date" Operator="DataTypeCheck" ErrorMessage="Invalid Date" ForeColor="Red" Display="Dynamic" />
            </div>

            <div class="filter-group">
                <label>To:</label>
                <asp:TextBox ID="dateTo2" placeholder="MM/DD/YYYY" runat="server" TextMode="Date" CssClass="modern-input" Width="320px"></asp:TextBox>
                <asp:CompareValidator ID="cv_dateTo2" runat="server" ControlToValidate="dateTo2" Type="Date" Operator="DataTypeCheck" ErrorMessage="Invalid Date" ForeColor="Red" Display="Dynamic" />
                <asp:Label ID="message3" ForeColor="red" runat="server"></asp:Label>
            </div>

            <asp:DropDownList ID="DW_DDL" runat="server" style="margin: 10px 0; padding: 5px;">
                <asp:ListItem Value="D">Deposits (D)</asp:ListItem>
                <asp:ListItem Value="W">Withdrawals (W)</asp:ListItem>
                <asp:ListItem Value="All" Selected>All</asp:ListItem>
            </asp:DropDownList>

            <div class="button-row">
                <asp:Button ID="listItem2" CssClass="modern-input-list" OnClick="DisplayTable2" runat="server" Text="View List" Width="100px"/>
                <asp:Button ID="viewDashboard2" CssClass="modern-input" runat="server" Text="View Dashboard" OnClick="BackToDashboard" Width="130px"/>
            </div>

            <div class="div_gridContainer">
                <asp:GridView ID="grdVwDWTransactions" CssClass="my-grid" runat="server" EmptyDataText="Select a date range and click 'List' to generate your statement." AutoGenerateColumns="false">  
                <EmptyDataRowStyle CssClass="empty-grid-message" Height="337px" />
                    <Columns>
                        <asp:BoundField DataField="SEQNO" HeaderText="Seq. #" />
                        <asp:BoundField DataField="TYPE" HeaderText="Type" />
                        <asp:BoundField DataField="DATE" HeaderText="Date"/>
                        <asp:BoundField DataField="AMOUNT" HeaderText="Amount"/>
                    </Columns>
                </asp:GridView>
            </div>
        </section>


        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <!-- Section 3: SEnt/Received Transactions -->
        <section class="report-section">
            <h1>My Sent or Received Transactions</h1>
            
            <div class="filter-group">
                <label>From:</label>
                <asp:TextBox ID="dateFrom3" placeholder="MM/DD/YYYY" runat="server" TextMode="Date" CssClass="modern-input" Width="320px"></asp:TextBox>
                <asp:CompareValidator ID="cv_dateFrom3" runat="server" ControlToValidate="dateFrom3" Type="Date" Operator="DataTypeCheck" ErrorMessage="Invalid Date" ForeColor="Red" Display="Dynamic" />
            </div>

            <div class="filter-group">
                <label>To:</label>
                <asp:TextBox ID="dateTo3" placeholder="MM/DD/YYYY" runat="server" TextMode="Date" CssClass="modern-input" Width="320px"></asp:TextBox>
                <asp:CompareValidator ID="cv_dateTo3" runat="server" ControlToValidate="dateTo3" Type="Date" Operator="DataTypeCheck" ErrorMessage="Invalid Date" ForeColor="Red" Display="Dynamic" />
                <asp:Label ID="message4" ForeColor="red" runat="server"></asp:Label>
            </div>

            <asp:DropDownList ID="SR_DDL" runat="server" style="margin: 10px 0; padding: 5px;">
                <asp:ListItem Value="S">Send (S)</asp:ListItem>
                <asp:ListItem Value="R">Receive (R)</asp:ListItem>
                <asp:ListItem Value="All" Selected>All</asp:ListItem>
            </asp:DropDownList>

            <div class="button-row">
                <asp:Button ID="listItem3" CssClass="modern-input-list" OnClick="DisplayTable3" runat="server" Text="View List" Width="100px"/>
                <asp:Button ID="viewDashboard3" CssClass="modern-input" runat="server" Text="View Dashboard" Width="130px" OnClick="BackToDashboard"/>
            </div>

            <div class="div_gridContainer">
               
                <asp:GridView ID="grdVwSRTransactions" CssClass="my-grid" runat="server" EmptyDataText="Select a date range and click 'List' to generate your statement." AutoGenerateColumns="false">
                <EmptyDataRowStyle CssClass="empty-grid-message" Height="337px" />
                    <Columns>
                        <asp:BoundField DataField="SEQNO" HeaderText="Seq. #" />
                        <asp:BoundField DataField="DATESENT" HeaderText="Date Sent" />
                        <asp:BoundField DataField="AMOUNT" HeaderText="Amount"/>
                        <asp:BoundField DataField="SENDTO" HeaderText="Sent To" />
                        <asp:BoundField DataField="RECEIVEFROM" HeaderText="Receive From" />
                    </Columns>
                </asp:GridView>
            </div>
        </section>
        <br />
        <br />
        <br />
        <br />

    </main>
</asp:Content>