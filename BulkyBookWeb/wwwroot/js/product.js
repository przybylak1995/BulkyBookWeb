

var dataTable;

$(document).ready(function () {
    loadDataTable();
    console.log("Test");
})

function loadDataTable() {

    dataTable = $('#tblData').DataTable(
        {


            //ajax: {"data" : [['test']]}

            "ajax": {
                "url": "/Product/Getall"
            },
            "columns": [

                { "data"    :   "name"  },
                { "data"    :   "isbn"  },
                { "data"    :   "price" },
                { "data": "category.name" },
                {
                    "data": "id",
                    "render": function (data) {

                        return `
                             <div class="w-75 btn-group" role="group">
                                <a href="/Product/Upsert?id=${data}"  class=" btn btn-primary mx-2" > <i class="bi bi-pencil-square"></i> Edit</a>
                                <a onClick=Delete('/Product/Delete?id=${data}') class="btn btn-secondary btn-danger " > <i class="bi bi-trash3"></i> Delete</a>
                             </div>
                            `
                    }
                },
                

              //{ "data": "author", "width": "15%" },
              //{ "data": "category", "width": "15%" },

            ]

        });

    /*C: \Users\krist\source\Udemy Projects\BulkyBookWeb\BulkyBookWeb\Controllers\ProductController.cs*/
}

function Delete(url) {

    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax(
                {
                    url: url,
                    type: "DELETE",
                    succes: function (data) {
                        if (data.succes) {
                            dataTable.ajax.reload();
                            toastr.error(data.message);
                        }
                        else {
                            toastr.error(data.message);
                        }
                    }
                })
        }
    })

}