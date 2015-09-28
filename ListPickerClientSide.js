function moveListBoxItems(firstListBoxId, secondListBoxId, selectedOnly) {
    var firstListBox = document.getElementById(firstListBoxId);
    var secondListBox = document.getElementById(secondListBoxId);
    
    if ((firstListBox != null) && (secondListBox != null)) {
        for (var i = firstListBox.options.length -1; i >= 0; i--) {
            if (selectedOnly) {
                if (firstListBox.options[i].selected) {
                    secondListBox.appendChild(firstListBox.options[i]);
                }
            }
            else {
                secondListBox.appendChild(firstListBox.options[i]);
                
            }
        }
    }
    return false;
}
