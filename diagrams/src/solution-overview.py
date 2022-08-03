from diagrams.azure.analytics import EventHubs
from diagrams.azure.compute import AppServices
from diagrams.azure.database import SQL
from diagrams.onprem.compute import Server

from diagrams import Cluster, Diagram, Edge

# for non-ms lock-in

with Diagram("ez2crawl overview", show=False):
    ez2on = Server("ez2on.co.kr")

    with Cluster("Event Flows"):
        with Cluster("Event Workers"):
            ez2crawlers = [
                AppServices("ez2crawler"),
            ]

        event_store = EventHubs("Azure Event Hub")

        with Cluster("Event Processing"):
            ez2on_processors = [
                AppServices("ez2on processor"),
            ]

    webapp_db = SQL("webapp db")

    ez2on \
    << Edge(color="red", label="daily 02:00 AM") \
    << ez2crawlers

    ez2crawlers \
    >> Edge(label="kafka protocol") \
    >> event_store

    event_store \
    << Edge(label="kafka protocol") \
    << ez2on_processors

    ez2on_processors >> webapp_db
