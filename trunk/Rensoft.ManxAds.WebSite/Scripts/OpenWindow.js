function openWindow(url, width, height) {
	xPosition = (screen.width) ? (screen.width - width) / 2 : 0
	yPosition = (screen.height) ? (screen.height - height) / 2 : 0
	settings = 'height=' + height + ',width=' + width + ',top=' +
	    yPosition + ',left=' + xPosition + ',scrollbars=yes'
	window.open(url, '', settings)
}
