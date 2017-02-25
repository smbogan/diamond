
$(function ()
{
    var path = $('table').attr('table-path');

    var tableSize = function () {
        var tbl = $("table");

        return {
            rows: parseInt(tbl.attr("table-rows")),
            cols: parseInt(tbl.attr("table-cols"))
        };
    }

    var currentCell = function () {
        var editingCell = $(".editing");

        if (editingCell.length === 0) {
            return null;
        }

        var row = parseInt(editingCell.attr("table-row"));
        var col = parseInt(editingCell.attr("table-col"));

        return {
            row: row,
            col: col
        };
    };

    var exitEditing = function (save) {
        var editingTD = $(".editing");
        
        

        var row = parseInt(editingTD.attr('table-row'));
        var col = parseInt(editingTD.attr('table-col'));

        var input = $(editingTD.find("input"));

        if (save) {
            window.controller.updateTableValue(path, row, col, input.val());
        }

        editingTD.off("blur");
        editingTD.empty();
        editingTD.removeClass("editing");

        var newTD = $(window.controller.getRenderedTableCell(path, row, col));

        editingTD.replaceWith(newTD);
    };

    var activateCell = function (row, col) {
        var target = $(".td-" + row + "-" + col);

        if (!target.is(".editing")) {

            var row = parseInt(target.attr('table-row'));
            var col = parseInt(target.attr('table-col'));

            var rawData = window.controller.getRawTableCell(path, row, col);

            var e = $('<input type="text" style="box-sizing: border-box; width: 100%;"></input>');
            e.val(rawData);

            target.empty();
            target.append(e);

            e.on("blur", function (event) {
                exitEditing(true);
            });

            e.keydown(function (event) {

                if (event.which === 13) { //enter
                    e.blur();
                } else if (event.which === 38) { //up
                    var cell = currentCell();

                    if (cell) {
                        if (cell.row > 0) {
                            exitEditing(true);
                            
                            activateCell(cell.row - 1, cell.col);
                        }
                    }

                    event.preventDefault();
                } else if (event.which === 40) { //down
                    var cell = currentCell();

                    if (cell) {
                        if (cell.row + 1 < tableSize().rows) {
                            exitEditing(true);

                            activateCell(cell.row + 1, cell.col);
                        }
                    }

                    event.preventDefault();
                } else if (event.which === 37) { //left
                    var cell = currentCell();
                    var inputElement = $($(".editing").find("input"));

                    if (inputElement.caret() === 0 && cell.col > 0) {
                        exitEditing(true);
                        activateCell(cell.row, cell.col - 1);
                        event.preventDefault();
                    }
                } else if (event.which === 39) { //right
                    var cell = currentCell();
                    var inputElement = $($(".editing").find("input"));

                    if (inputElement.caret() === inputElement.val().length && cell.col + 1 < tableSize().cols) {
                        exitEditing(true);
                        activateCell(cell.row, cell.col + 1);
                        event.preventDefault();
                    }
                }
            });

            e.focus();

            target.addClass("editing");
        }
    };

    $(document).keyup(function (e) {
        if (e.keyCode == 27) { //escape
            exitEditing(false);
        }
    });

    $(document).on("click", "td.td-cell", function (event) {
        var e = $(event.target);
        activateCell(e.attr("table-row"), e.attr("table-col"));
    });

    
    $(".save-table-button").click(function () {
        window.controller.saveTable(path);
    });

    $(".add-row-button").click(function () {
        window.controller.addTableRow(path);
        document.location.reload(true);
    });

    $.contextMenu({
        selector: '.row-handle',
        callback: function (key, options) {
            var target = $(this);

            var currentRow = parseInt(target.parent().attr("row"));

            var reload = true;

            switch (key) {
                case "add-above":
                    window.controller.addTableRowAbove(path, currentRow);
                    break;
                case "add-below":
                    window.controller.addTableRowBelow(path, currentRow);
                    break;
                case "copy":
                    window.controller.copyTableRow(path, currentRow);
                    break;
                case "paste-above":
                    window.controller.pasteTableRowAbove(path, currentRow);
                    break;
                case "paste-below":
                    window.controller.pasteTableRowBelow(path, currentRow);
                    break;
                case "delete":
                    window.controller.deleteTableRow(path, currentRow);
                    break;
                default:
                    reload = false;
                    break;
            }

            if (reload) {
                document.location.reload(true);
            }
        },
        items: {
            "add-above": { name: "Add Row Above", icon: "add" },
            "add-below": { name: "Add Row Below", icon: "add" },
            "copy": { name: "Copy", icon: "copy" },
            "paste-above": { name: "Paste Row Above", icon: "paste" },
            "paste-below": { name: "Paste Row Below", icon: "paste" },
            "sep1": "---------",
            "delete": { name: "Delete", icon: "delete" }
        }
    });
});