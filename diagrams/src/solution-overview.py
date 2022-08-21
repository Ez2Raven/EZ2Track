from diagrams.azure.analytics import EventHubs
from diagrams.azure.compute import AppServices
from diagrams.azure.database import SQL
from diagrams.onprem.compute import Server

from diagrams import Cluster, Diagram, Edge

# for non-ms lock-in

with Diagram("EZ2Track Solution Overview", show=False):
    ez2on = Server("wikiwiki.jp/ez2on")
    notification_telegram = Server("Telegram")

    with Cluster("Microsoft Azure"):
        with Cluster("Event Flows"):
            with Cluster("Producers"):
                ez2crawlers = [
                    AppServices("EZ2Crawler"),
                ]

            event_store = EventHubs("Azure Event Hub")
            ez2track_workflow = AppServices("EZ2Track\n Workflow")
            with Cluster("Processors"):
                ez2on_processors = [
                    ez2track_workflow
                ]

        ez2track_api = AppServices("EZ2Track API")
        ez2track_web = AppServices("Ez2Track\n Blazor Client")

        webapp_db = SQL("EZ2Track db")

        ez2on \
        << Edge(color="red", label="daily 02:00 AM") \
        << ez2crawlers

        ez2crawlers \
        >> Edge(label="kafka protocol") \
        >> event_store

        event_store \
        << Edge(label="kafka protocol") \
        << ez2on_processors

        ez2track_workflow >> webapp_db << ez2track_api << ez2track_web

    ez2track_workflow \
    >> Edge(color="red") \
    << notification_telegram
