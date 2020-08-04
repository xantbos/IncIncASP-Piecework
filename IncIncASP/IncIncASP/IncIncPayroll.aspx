<%@ Page Title="Payroll Entry" Language="C#" AutoEventWireup="true" MasterPageFIle="~/IncInc.Master" CodeBehind="IncIncPayroll.aspx.cs" Inherits="IncIncASP.IncIncPayroll" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-4">
            <p>
                Worker Name:<br />
                <asp:TextBox ID="txtWorkerNameEntry" AccessKey="N" runat="server" ToolTip="Enter the Worker's first and last name" /><br />
                <!--<asp:RequiredFieldValidator runat="server"
                    ControlToValidate="txtWorkerNameEntry"
                    ErrorMessage="You must enter a first and last name." />-->
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
                Messages:<br />
                <asp:TextBox ID="txtMessagesEntry" runat="server" AccessKey="M" ToolTip="Enter the Worker's messages sent" /><br />
                <asp:RequiredFieldValidator ControlToValidate="txtMessagesEntry" runat="server" ErrorMessage="Messages is a required field."/> <br />
                <asp:RangeValidator ControlToValidate="txtMessagesEntry" runat="server" ErrorMessage="Messages must be a whole number between 0 and 20000" MinimumValue="0" Type="Double" MaximumValue="20000" />
            </p>
            <asp:Panel ID="pnlHourly" runat="server" Visible="false">
                <p>
                    Hours Worked:<br />
                    <asp:TextBox ID="txtHoursWorkedEntry" runat="server" AccessKey="H" ToolTip="Enter the amount of hours this Worker worked" /><br />
                    <asp:RequiredFieldValidator ControlToValidate="txtHoursWorkedEntry" runat="server" ErrorMessage="Hours Worked is a required field."/> <br />
                    <asp:RangeValidator ControlToValidate="txtHoursWorkedEntry" runat="server" ErrorMessage="You must enter a whole number between 1 and 168." MinimumValue="1" MaximumValue="168" Type="Integer" />
                </p>
                <p>
                    Hourly Pay:<br />
                    <asp:TextBox ID="txtHourlyPayEntry" runat="server" AccessKey="P" ToolTip="Enter the $/h for this Worker" /><br />
                    <asp:RequiredFieldValidator ControlToValidate="txtHourlyPayEntry" runat="server" ErrorMessage="Hourly Pay is a required field."/> <br />
                    <asp:RegularExpressionValidator ID="revNumber" runat="server" ControlToValidate="txtHourlyPayEntry"
                    ErrorMessage="You must enter a valid currency amount (# or *#.##)." ValidationExpression="^\$?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$" />
                </p>
            </asp:Panel>
            <p>
                <asp:Button ID="btnCalculate" runat="server" Text="Calculate" ToolTip="Calculate this worker's pay" OnClick="btnCalculate_OnClick" AccessKey="C" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" ToolTip="Clear the form to enter another worker's data" OnClick="btnClear_OnClick" AccessKey="L" CausesValidation="false" />
            </p>
        </div>
        <div class="col-md-4">
            <asp:RadioButtonList ID="rdWorkerType" runat="server" OnSelectedIndexChanged="rdWorkerType_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Text="Regular Worker" Selected="true"/>
                <asp:ListItem Text="Senior Worker" />
                <asp:ListItem Text="Hourly Worker" />
            </asp:RadioButtonList>
        </div>
        <div class="col-md-4">
            <ul class="list-group list-group-flush">
            <li class="list-group-item">Worker's Pay: <asp:Label ID="lblWorkerPayOutput" runat="server" /></li>
            <li class="list-group-item">Total Workers Paid: <asp:Label ID="lblWorkerCountString" runat="server" /> <asp:Label ID="lblWorkerCountOutput" runat="server" /></li>
            <li class="list-group-item">Total Messages: <asp:Label ID="lblTotalMessagesString" runat="server" /> <asp:Label ID="lblTotalMessagesOutput" runat="server" /></li>
            <li class="list-group-item">Overall Pay: <asp:Label ID="lblOverallPayString" runat="server" /> <asp:Label ID="lblOverallPayOutput" runat="server" /></li>
            <li class="list-group-item">Average Pay: <asp:Label ID="lblOverallAverageString" runat="server" /> <asp:Label ID="lblOverallAverageOutput" runat="server" /></li>
            </ul><br />
            <span class="float-right"><asp:Button ID="btnResetTotals" runat="server" Text="Reset Totals" ToolTip="Reset stored totals" OnClick="btnResetTotals_OnClick" CausesValidation="False"/></span>
        </div>
        <p>
            <asp:Label ID="lblOutNote" runat="server" />
        </p>
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
