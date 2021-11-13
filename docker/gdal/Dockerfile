FROM osgeo/gdal

# We copy just the requirements.txt first to leverage Docker cache
COPY ./script/app.py  /script/app.py
COPY ./script/from_shapefile  /script/from_shapefile
COPY ./script/to_GeoJSON  /script/to_GeoJSON 

WORKDIR /script

ENTRYPOINT [ "python" ]

CMD [ "app.py" ]