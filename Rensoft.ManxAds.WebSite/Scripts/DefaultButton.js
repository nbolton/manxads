function clickButton(e, id){
    var button = document.getElementById(id)
    var keyCode = 0
    if (typeof button != 'object'){
        return true
    }
    if (navigator.appName.indexOf("Netscape")>(-1)){ 
        keyCode = e.keyCode
    }
    if (navigator.appName.indexOf("Microsoft Internet Explorer")>(-1)){ 
        keyCode = event.keyCode
    }
    if (keyCode == 13){
        button.click()
        return false
    }
}