<%@ Page Title="MARREG | Employee Directory" Language="VB" MasterPageFile="~/MARREG_PORTAL.master" AutoEventWireup="false" CodeFile="MARREG_Employee_Directory.aspx.vb" Inherits="MARREG_Portal_MARREG_Employee_Directory" EnableViewStateMac="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .employee-card .card-body {
            padding: 1rem;
        }

        .employee-card h6 {
            margin-bottom: 0.5rem;
            font-weight: 600;
        }

        .firstcard {
            padding-left: 15px !important;
            padding-right: 15px !important;
            margin-top: 10px !important;
            margin-bottom: 10px !important;
            padding-bottom: 0 !important;
        }

        #officials .card-body,
        #sp .card-body,
        #cu .card-body {
            padding-top: 10px !important;
            padding-bottom: 5px !important;
        }

        /* Minimum white space inside cards */
        .employee-card-common .card-body {
            padding: 0.65rem 0.75rem !important; /* reduced from 1rem */
        }

        /* Equal height fix */
        .equal-row {
            display: flex;
            flex-wrap: wrap;
        }

            .equal-row > div {
                display: flex;
                margin-bottom: 10px !important;
            }

        .employee-card-common {
            flex: 1;
            display: flex;
            flex-direction: column;
        }

        .employee-card h6 {
            margin-bottom: 0.35rem; /* tighter */
            font-weight: 600;
        }

        .employee-official {
            background-color: #F3FAFE !important;
            box-shadow: 0 4px 10px rgba(0,0,0,0.20) !important;
        }

        .employee-software {
            background-color: #E8F5E9 !important;
            box-shadow: 0 4px 10px rgba(0,0,0,0.20) !important;
        }

        .employee-contract {
            background-color: #FFF9C4 !important;
            box-shadow: 0 4px 10px rgba(0,0,0,0.20) !important;
        }

        .employee-card-common > .card-body {
            margin: initial;
        }
    </style>
    <script type="text/javascript" src="../MARREG_Scripts/Alternate.js"></script>
    <link rel="stylesheet" type="text/css" href="../bootstrap/toast/toastr.css" />
    <script type="text/javascript" src="../bootstrap/toast/toastr.js"></script>
    <script type="text/javascript" src="../bootstrap/fontawesome/js/all.min.js"></script>
    <script src="../bootstrap/js/libs/bootstrap.min.js"></script>
    <script src="../bootstrap/js/libs/bootbox.min.js"></script>
    <link rel="stylesheet" type="text/css" href="../bootstrap/css/libs/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="../bootstrap/fontawesome/css/all.min.css" />
    <link rel="stylesheet" type="text/css" href="../bootstrap/css/style.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="pheading">Employee Directory</div>
        <div class="container">
            <div id="accordion">
                <div class="card">
                    <div class="card-header col-12">
                        <a class="collapsed card-link action_link fw-bold text-primary"
                            data-bs-toggle="collapse"
                            href="#officials"
                            role="button"
                            aria-expanded="false"
                            aria-controls="officials">Officials
                        </a>
                    </div>
                    <div id="officials"
                        class="collapse ps-2 w-100"
                        data-bs-parent="#accordion">
                        <div class="card-body">
                            <div class="card top-border card-body firstcard p-3">
                                <div class="row g-3 equal-row">

                                    <asp:Repeater ID="rptOfficials" runat="server">
                                        <ItemTemplate>
                                            <div class="col-lg-4 col-md-6 col-sm-12 mb-3">
                                                <div class="card shadow-sm h-100 border-0 employee-card-common employee-official">
                                                    <div class="card-body">
                                                        <h6 class="text-primary mb-1 text-uppercase">
                                                            <strong><%# Eval("emp_name") %></strong>
                                                        </h6>
                                                        <p class="mb-1"><strong>Post:</strong> <%# Eval("emp_post") %></p>
                                                        <p class="text-muted text-uppercase">
                                                            <strong>Power & Function:</strong>
                                                            <%# StrConv(Eval("emp_pwrandfunc"), VbStrConv.ProperCase) %>
                                                        </p>
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card">
                    <div class="card-header col-12">
                        <a class="collapsed card-link action_link fw-bold text-primary"
                            data-bs-toggle="collapse"
                            href="#sp"
                            role="button"
                            aria-expanded="false"
                            aria-controls="sp">Software Personnels (Appointment under 5859-F(Y))
                        </a>
                    </div>
                    <div id="sp"
                        class="collapse ps-2 w-100"
                        data-bs-parent="#accordion">
                        <div class="card-body">
                            <div class="card top-border card-body firstcard p-3">
                                <div class="row g-3 equal-row">

                                    <asp:Repeater ID="rptSftrPrsnl" runat="server">
                                        <ItemTemplate>
                                            <div class="col-lg-4 col-md-6 col-sm-12 mb-3">
                                                <div class="card shadow-sm h-100 border-0 employee-card-common employee-software">
                                                    <div class="card-body">
                                                        <h6 class="text-primary mb-1 text-uppercase">
                                                            <strong><%# Eval("emp_name") %></strong>
                                                        </h6>
                                                        <p class="mb-1"><strong>Post:</strong> <%# Eval("emp_post") %></p>
                                                        <p class="text-muted text-uppercase">
                                                            <strong>Power & Function:</strong>
                                                            <%# StrConv(Eval("emp_pwrandfunc"), VbStrConv.ProperCase) %>
                                                        </p>
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="card">
                    <div class="card-header col-12">
                        <a class="collapsed card-link action_link fw-bold text-primary"
                            data-bs-toggle="collapse"
                            href="#cu"
                            role="button"
                            aria-expanded="false"
                            aria-controls="cu">Contractual Employee
                        </a>
                    </div>
                    <div id="cu"
                        class="collapse ps-2 w-100"
                        data-bs-parent="#accordion">
                        <div class="card-body">
                            <div class="card top-border card-body firstcard p-3">
                                <div class="row g-3 equal-row">

                                    <asp:Repeater ID="rptContract" runat="server">
                                        <ItemTemplate>
                                            <div class="col-lg-4 col-md-6 col-sm-12 mb-3">
                                                <div class="card shadow-sm h-100 border-0 employee-card-common employee-contract">
                                                    <div class="card-body ">
                                                        <h6 class="text-primary mb-1 text-uppercase">
                                                            <strong><%# Eval("emp_name") %></strong>
                                                        </h6>
                                                        <p class="mb-1"><strong>Post:</strong> <%# Eval("emp_post") %></p>
                                                        <p class="text-muted text-uppercase">
                                                            <strong>Power & Function:</strong>
                                                            <%# StrConv(Eval("emp_pwrandfunc"), VbStrConv.ProperCase) %>
                                                        </p>
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>

                                </div>
                            </div>

                        </div>
                    </div>
                </div>

            </div>


        </div>
    </div>

    <%--<script src="https://cdn.jsdelivr.net/npm/jquery@3.2.1/dist/jquery.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>--%>
</asp:Content>
