<%@ Page Title="Worker List" Language="C#" MasterPageFile="~/IncInc.Master" AutoEventWireup="true" CodeBehind="Table.aspx.cs" Inherits="IncIncASP.Table" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <br />
    <div class="col-md-6">
        <asp:Table ID="tblWorker" runat="server" CellPadding="5" CellSpacing="5" GridLines="Horizontal">
            <asp:TableHeaderRow runat="server">
                <asp:TableHeaderCell></asp:TableHeaderCell>
                <asp:TableHeaderCell>Name</asp:TableHeaderCell>
                <asp:TableHeaderCell>Messages</asp:TableHeaderCell>
                <asp:TableHeaderCell>Pay</asp:TableHeaderCell>
                <asp:TableHeaderCell>Entry</asp:TableHeaderCell>
                <asp:TableHeaderCell>Type</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
        <br />
        <asp:Panel ID="pnlTableManipulation" runat="server" Visible="false">
            <p>
                Worker Name:<br />
                <asp:TextBox ID="txtWorkerNameEntry" runat="server" Enabled="false" ToolTip="Enter the updated Worker Name here." />
                    <br />
                    <asp:CustomValidator ID="NameValidator" runat="server" 
                            ControlToValidate="txtWorkerNameEntry"
                            ClientValidationFunction="ClientNameValidate()" 
                            OnServerValidate="ServerNameValidate"
                            ValidateEmptyText="true"
                            Display="Static"
                            ErrorMessage="You must enter a first and last name." />
                    <br />
                    <asp:CustomValidator ID="NameLengthValidator" runat="server" 
                            ControlToValidate="txtWorkerNameEntry"
                            ClientValidationFunction="ClientNameLengthValidate()" 
                            OnServerValidate="ServerNameLengthValidate"
                            ValidateEmptyText="true"
                            Display="Static"
                            ErrorMessage="First/Last names cannot be less than 2 characters." />
            </p>
            <p>
                Messages: <br />
                <asp:TextBox ID="txtMessagesEntry" runat="server" enabled="false" ToolTip="Enter the updated Message count here."/>
                    <br />
                    <asp:RequiredFieldValidator ControlToValidate="txtMessagesEntry" runat="server" ErrorMessage="Messages is a required field."/> <br />
                    <asp:RangeValidator ControlToValidate="txtMessagesEntry" runat="server" ErrorMessage="Messages must be a whole number between 0 and 20000" MinimumValue="0" Type="Double" MaximumValue="20000" />
            </p>
            <asp:Panel ID="pnlHourly" runat="server" Visible="false">
                <p>
                    Hours Worked:<br />
                    <asp:TextBox ID="txtHoursWorkedEntry" runat="server" Enabled="false" ToolTip="Enter the updated Hours Worked here."/><br />
                        <asp:RequiredFieldValidator ControlToValidate="txtHoursWorkedEntry" runat="server" ErrorMessage="Hours Worked is a required field."/> <br />
                       <asp:RangeValidator ControlToValidate="txtHoursWorkedEntry" runat="server" ErrorMessage="You must enter a whole number between 1 and 168." MinimumValue="1" MaximumValue="168" Type="Integer" />
                </p>
                <p>
                    Hourly Pay:<br />
                    <asp:TextBox ID="txtHourlyPayEntry" runat="server" Enabled="false" ToolTip="Enter the updated Hourly Pay here."/><br />
                        <asp:RequiredFieldValidator ControlToValidate="txtHourlyPayEntry" runat="server" ErrorMessage="Hourly Pay is a required field."/> <br />
                        <asp:RegularExpressionValidator ID="revNumber" runat="server" ControlToValidate="txtHourlyPayEntry"
                        ErrorMessage="You must enter a valid currency amount (# or *#.##)." ValidationExpression="^\$?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$" />
                </p>
            </asp:Panel>
            <asp:Button ID="btnUpdate" OnClick="UpdateWorker" CausesValidation="true" runat="server" Enabled="false" Text="Update Worker"/>
            <asp:Button ID="btnDelete" OnClick="DeleteWorker" CausesValidation="true" runat="server" Enabled="false" Text="Delete Worker"/>
        </asp:Panel>
        <br />
        <asp:Label ID="lblOutMessage" runat="server" />
    </div>
    <script> 
        function ClientNameValidate(source, args)
        {
            var parts = args.split(' ');
            if (parts.length > 1)
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
        }
        function ClientNameLengthValidate(source, args) {
            var parts = args.split(' ');
            args.IsValid = true;
            for (var s in parts)
            {
                if(s.length <= 2)
                {
                    args.IsValid = false;
                    break;
                }
            }
        }
    </script>
</asp:Content>
