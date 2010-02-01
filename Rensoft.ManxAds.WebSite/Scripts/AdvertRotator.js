var advertGroups = new Array()
var enumerators = new Array()
var startTimes = new Array()

function AddGroup(group) {
    advertGroups[group] = new Array()
    enumerators[group] = 0
}

function Add(group, id, timer) {
    advertGroups[group][id] = timer
}

function Now() {
    // time in ms since epoch
    return new Date().getTime();
}

function Rotate(group) {

    debug = document.getElementById("ctl00_AdvertDebug").value
    
    // Get current enumerator.
    i = enumerators[group]

    // construct advert id for last advert
    advertId = group + "_" + i
    
    // Hide current advert.
    advert = document.getElementById(advertId)
    advert.style.visibility = "hidden"
    advert.style.zIndex = -100 // ensure it's at the back

    if (debug == 'True') {
        // hide current advert debug
        debugViewElem = document.getElementById(advertId + '_debug')
        debugViewElem.style.visibility = "hidden"
    }
    
    // Increment or reset.
    if (i++ >= advertGroups[group].length - 1) {
        //alert("reached end")
        i = 0
    }
    
    // construct advert id for this advert
    advertId = group + "_" + i
    
    // Display next advert.
    timer = advertGroups[group][i]
    advert = document.getElementById(advertId)
    advert.style.visibility = "visible"
    advert.style.zIndex = 100 // ensure it's at the front
    
    // queue the next advert to rotate
    setTimeout("Rotate('" + group + "')", timer * 1000)
    
    // If flash, restart the animation.
    flash = document.getElementById(advertId + "_Flash")
    if (flash != null) {
        flash.Rewind()
        flash.Play()
    }

    // display some debugging info
    if (debug == 'True') {
        debugViewElem = document.getElementById(advertId + '_debug')
        debugViewElem.style.visibility = "visible"
        debugViewElem.innerHTML = 'loading...'

        // record when the advert started
        startTimes[advertId] = Now()

        DebugUpdate(group, i)
    }
    
    // Update group enumerator.
    enumerators[group] = i
}

function DebugUpdate(group, i) {

    // construct advert id
    advertId = group + "_" + i

    debugViewElem = document.getElementById(advertId + '_debug')

    timer = advertGroups[group][i]
    started = startTimes[advertId]
    running = Now() - started

    debugViewElem.innerHTML =
        "timer: " + timer + "s" + "<br />" +
        "running: " + Math.round(running / 1000) + "s"

    setTimeout("DebugUpdate('" + group + "', '" + i + "')", 1000)
}