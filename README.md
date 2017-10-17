# EncodedPolyline
A demo application of a Xamarin Forms project using Xamarin Forms Maps displaying an encoded polyline

# The application
When first running the application the map is centered around the users geolocation using the geolocator plugin.

You then have two buttons, one for changing the polyline, one for changing the polyline's colour.

# Interesting parts

In the shared project you will find a subclass of a Xamarin.Forms.Maps Map which adds support for a Google style encoded polyline and setting its colour.
The decoding of the polyline happens in this subclass and a dialog between this Forms control and it's custom renderer is established in order to properly display the map.

# Encoded polyline format

You can find more information about the Google's encoded polyline format on this website:
(https://developers.google.com/maps/documentation/utilities/polylinealgorithm?hl=en)[https://developers.google.com/maps/documentation/utilities/polylinealgorithm?hl=en]

# Polyline example

```
kegiH}{dMv`@mhBhKzDcJtx@xFfFmRj_AsJiGg@wB}FsDoAv@eLmH~DoQvGhFu@jE
```

# Android

Please don't forget to add your own Google Maps Android API key in the Manifest before running the app !

# Reference

I found where to decode the encoded polyline there [https://forums.xamarin.com/discussion/85684/how-can-i-draw-polyline-for-an-encoded-points-string](https://forums.xamarin.com/discussion/85684/how-can-i-draw-polyline-for-an-encoded-points-string)

Thanks Luiz Gustavo !
