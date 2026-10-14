<%@ Page Title="MARREG | RTI Authority Directory" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="MARREG_RTI_Directory.aspx.vb" Inherits="MARREG_Portal_MARREG_RTI_Directory" EnableViewStateMac="true" %>

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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="pheading">RTI Authority Officials</div>
    <div class="container">
     <div class="card-body">
                        <div class="card top-border card-body firstcard p-3">
                            <div class="row g-3 equal-row">

                         <asp:Repeater ID="rptRtiPrsnl" runat="server">
                                    <ItemTemplate>
                                        <div class="col-lg-4 col-md-6 col-sm-12 mb-3">
                                            <div class="card shadow-sm h-100 border-0 employee-card-common employee-official">
                                                <div class="card-body">
                                                    <h6 class="text-primary mb-1 text-uppercase">
                                                        <strong><%# Eval("emp_post") %></strong>
                                                    </h6>
                                                    <p class="mb-1 text-uppercase"><strong>Name:</strong> <%# Eval("emp_name") %></p>
                                                    <p class="text-muted">
                                                        <strong>Designation:</strong>
                                                        <%# StrConv(Eval("emp_desig"), VbStrConv.ProperCase) %>
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
</asp:Content>