FROM osgeo/gdal

ADD scripts/ /scripts
WORKDIR /scripts
COPY /scripts/app.py ./
CMD python app.py path_to_geojson path_to_shapefile





#USER root
#RUN chmod +x /scripts/app.sh
#ENTRYPOINT ["/scripts/app.sh"]
