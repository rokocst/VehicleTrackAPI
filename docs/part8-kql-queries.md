# Part 8 — Log Analytics Queries (Azure Application Insights)
## PROG3176 Final Project — VehicleTrack API

Run these queries in Azure Portal → Application Insights → Logs

---

### Query 1: Recent requests (Part 8.4)
```kql
requests
| take 20
```

### Query 2: Traces ordered by timestamp (Part 8.4)
```kql
traces
| order by timestamp desc
```

### Query 3: Failed requests
```kql
requests
| where success == false
| project timestamp, name, resultCode, duration
| order by timestamp desc
```

### Query 4: Average response time by endpoint
```kql
requests
| summarize avg(duration) by name
| order by avg_duration desc
```

### Query 5: Request count per hour
```kql
requests
| summarize count() by bin(timestamp, 1h)
| order by timestamp desc
```

---

## Part 8.6 — Alert Rule Setup

In Azure Portal → Application Insights → Alerts → Create:

**Alert: High Response Time**
- Signal: `requests/duration`
- Condition: Average > 1000ms (1 second)
- Evaluation period: 5 minutes
- Frequency: Every 1 minute
- Severity: Warning (Sev 2)
- Action group: Email notification

---

## Part 8.5 — Live Metrics & Application Map

- **Live Metrics**: Application Insights → Live Metrics  
  Shows real-time incoming requests, failures, CPU, memory while API is running.

- **Application Map**: Application Insights → Application Map  
  Shows component topology and dependency health.
