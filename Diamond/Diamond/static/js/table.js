$(function ()
{
    $(document).keyup(function (e) {
        if (e.keyCode == 27) { //escape
            var editingTD = $(".editing");
            editingTD.off("blur");
            editingTD.empty();
            editingTD.removeClass("editing");

            var path = $('table').attr('table-path');

            var row = parseInt(editingTD.attr('table-row'));
            var col = parseInt(editingTD.attr('table-col'));

            var newTD = $(window.controller.getRenderedTableCell(path, row, col));

            editingTD.replaceWith(newTD);
        }
    });

    $(document).on("click", "td.td-cell", function (event) {
 
        var target = $(event.currentTarget);

        if (!target.is(".editing")) {
            var path = $('table').attr('table-path');


            var row = parseInt(target.attr('table-row'));
            var col = parseInt(target.attr('table-col'));

            var rawData = window.controller.getRawTableCell(path, row, col);

            var e = $('<input type="text"></input>');
            e.val(rawData);

            target.empty();
            target.append(e);

            e.on("blur", function (event) {
                var inputTarget = $(event.currentTarget);

                window.controller.updateTableValue(path, row, col, e.val());

                var newTD = $(window.controller.getRenderedTableCell(path, row, col));

                target.replaceWith(newTD);
            });

            e.keydown(function (event) {
                console.log("key: " + event.which);
                if (event.which === 13) { //enter
                    e.blur();
                } else if (event.which === 38) { //up
                    event.preventDefault();
                } else if (event.which == 40) { //down
                    event.preventDefault();
                }
            });

            e.focus();

            target.addClass("editing");
        }
    });

    
});