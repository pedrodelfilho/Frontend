$(document).ready(function () {
 

   

    $('#btnEnviar').click(function (e) {
        e.preventDefault();

        var email = document.getElementById('inputEmail').value;
        if (email !== '') {
            $.ajax({
                url: '/Autenticacao/EsqueciaSenha',
                type: 'POST',
                data: { Email: email },
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8'
            }).done(function (response) {
                $('#Message').modal('show')
                console.log(response)
                document.getElementById("MessageTitleLabel").innerHTML = response['title'];
                document.getElementById("MessageLabel").innerHTML = response['message'];
            });
        }
    });
});

