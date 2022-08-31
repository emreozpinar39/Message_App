
$(() => {

    var connection = new signalR.HubConnectionBuilder().withUrl("/signalserver").build();

    connection.start();

    connection.on("LoadMessages", function () {
        LoadData();
    })


    LoadData();

    function LoadData() {
        var tr = '';

        $.ajax({
            url: '/message/GetMessages',
            method: 'GET',
            success: (result) => {
                $.each(result, (k, v) => {
                    tr += `<tr>
                            <td>${v.User.FirstName}</td>
                            <td>${v.Content}</td>
                            <td>${v.MessageDate}</td>
                            <td>
                            <a href='../Message/Edit?id=${v.Id}'>Edit</a>
                            <a href='../Message/Details?id=${v.Id}'>Details</a>
                            <a href='../Message/Delete?id=${v.Id}'>Delete</a>
                            </td>
                        
                    </tr>`
                })

                $("#tableBody").html(tr);
            },
            error: (error) => {
                console.log(error)
            }
        });
    }


})

