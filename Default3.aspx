<%@ Page Title="" Language="VB" MasterPageFile="~/MARREG_MasterData.master" AutoEventWireup="false" CodeFile="Default3.aspx.vb" Inherits="Default3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="bootstrap/js/libs/jquery.min.js"></script>
    <script src="bootstrap/js/libs/bootstrap.min.js"></script>
    <script src="bootstrap/js/libs/bootbox.min.js"></script>
    <script src="bootstrap/js/libs/moment.min.js"></script>
    <script src="bootstrap/js/libs/bootstrap-datetimepicker.min.js"></script>
    <script src="bootstrap/fontawesome/js/all.js"></script>
    <script src="bootstrap/js/libs/datatables.min.js"></script>
    <script src="bootstrap/js/libs/dataTables.checkboxes.min.js"></script>

    <link rel="stylesheet" type="text/css" href="bootstrap/css/libs/bootstrap.min.css" crossorigin="anonymous" />
    <link rel="stylesheet" type="text/css" href="bootstrap/css/libs/bootstrap-datetimepicker.css" />
    
    <link rel="stylesheet" type="text/css" href="bootstrap/fontawesome/css/all.min.css" />
    <link rel="stylesheet" type="text/css" href="bootstrap/css/libs/datatables.min.css" />
    <link rel="stylesheet" type="text/css" href="bootstrap/css/libs/dataTables.bootstrap4.min.css" />
    <link rel="stylesheet" type="text/css" href="bootstrap/css/style.css" />
    <link rel="stylesheet" type="text/css" href="bootstrap/css/libs/dataTables.checkboxes.css" />
    <script>
        $(function () {
            var url = "";
            $.fn.dataTable.ext.errMode = 'none';
            var table = $('#example').DataTable({
                searching: false, paging: false, info: false,
                'ajax': 'new.txt',
                'columnDefs': [
                    {
                        'targets': 0,
                        'visible': false,
                    },
                    {
                        'targets': 1,
                        'data':'place',
                    },
                    {                        
                        'targets': [2],
                        'data': 'id',
                        //'checkboxes': true,
                        'checkboxes': {
                            'selectRow': true
                        },
                        'createdCell': function (td, cellData, rowData, row, col) {
                            if (rowData['hm'] === 'Y') {
                                this.api().cell(td).checkboxes.select();
                            }
                        }
                    },
                    {
                        'targets': [3],
                        'data': 'id',
                        'checkboxes': true,
                        //'checkboxes': {
                        //    'selectRow': true
                        //},
                        'createdCell': function (td, cellData, rowData, row, col) {
                            if (rowData['sm'] === 'Y') {
                                this.api().cell(td).checkboxes.select();
                            }
                        }
                    }
                ],
                'select': {
                    'style': 'multi'
                },
                'order': [[1, 'asc']]
            });
            $("#save").click(function (e) {
                e.preventDefault();
                var arr = []
                $('#example > tbody  > tr').each(function (index, tr) {
                    var d = {}
                    var row = $(this);
                    var data = $('#example').DataTable().row(row).data();
                    var id = data['id'];
                    var s1 = $(row).children('td:nth-child(2)').find("[type=checkbox]").prop('checked');
                    var s2 = $(row).children('td:nth-child(3)').find("[type=checkbox]").prop('checked');

                    d = { id: id, hm: s1?'Y':'N', sm: s2?'Y':'N' };
                    arr.push(d);
                });

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%--<form>--%>
    <div class="container">
        <div class="pheading">Jurisdiction Update</div>
        <div class="container">
            <div class="card top-border card-body">
                <div class="form-group row">
                    <div class="col-md-6">
                        <div class="datatbl table-responsive">
                            <table id="example" class="display  table-striped table-hover">
                               <thead>
                                  <tr>
                                      <th rowspan="3"></th>
                                      <th rowspan="3">Jurisdiction</th>
                                  </tr>
                                  <tr>
                                     <th>Hindu</th>
                                     <th>Special</th>
                                  </tr>
                                  <tr>
                                     <th></th>
                                     <th></th>
                                  </tr>
                               </thead>
                               <tfoot>
                                  <tr>
                                     <th></th>
                                     <th>Name</th>
                                     <th>Hindu</th>
                                     <th>Special</th>
                                  </tr>
                               </tfoot>
                            </table>
                        </div>
                    </div>
                    <div class="col-6">
                        <pre id="example-console-rows"></pre>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-6 text-center ">
                        <button type="submit" class="btn btn-primary btn-sm" id="save"><i class="fa fa-save"></i>&nbsp;Save</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
        <%--</form>--%>
</asp:Content>

