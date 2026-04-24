/**
 * VehicleTrack API - Console Client
 * PROG3176 Final Project — Part 4
 *
 * Calls two endpoints:
 *   1. GET /vehicles        (CRUD endpoint)
 *   2. GET /vehicles/stats  (non-CRUD endpoint)
 *
 * Usage:
 *   node client.js                        (uses localhost:8080 by default)
 *   node client.js https://your-azure-url (uses deployed Azure URL)
 */

const BASE_URL = process.argv[2] || "http://localhost:8080";

async function callEndpoint(label, path) {
  const url = `${BASE_URL}${path}`;
  console.log(`\n${"─".repeat(60)}`);
  console.log(`📡  ${label}`);
  console.log(`    GET ${url}`);
  console.log("─".repeat(60));

  try {
    const response = await fetch(url);

    if (!response.ok) {
      console.error(`❌  HTTP ${response.status}: ${response.statusText}`);
      return;
    }

    const data = await response.json();
    console.log(JSON.stringify(data, null, 2));
    console.log(`\n✅  Success — HTTP ${response.status}`);
  } catch (err) {
    console.error(`❌  Request failed: ${err.message}`);
    console.error(`    Make sure the API is running at ${BASE_URL}`);
  }
}

async function createVehicle() {
  const url = `${BASE_URL}/vehicles`;
  const newVehicle = {
    vin: "TEST123456789VIN1",
    make: "Chevrolet",
    model: "Silverado",
    year: 2022,
    mileage: 15000,
    status: "Active",
  };

  console.log(`\n${"─".repeat(60)}`);
  console.log(`📡  Create a new vehicle (CRUD — POST)`);
  console.log(`    POST ${url}`);
  console.log("─".repeat(60));
  console.log("Request body:", JSON.stringify(newVehicle, null, 2));

  try {
    const response = await fetch(url, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(newVehicle),
    });

    const data = await response.json();
    console.log("\nResponse:", JSON.stringify(data, null, 2));
    console.log(`\n✅  Success — HTTP ${response.status}`);
    return data.id;
  } catch (err) {
    console.error(`❌  Request failed: ${err.message}`);
  }
}

async function main() {
  console.log("╔══════════════════════════════════════════════════════════╗");
  console.log("║         VehicleTrack API — Console Client                ║");
  console.log("║         PROG3176 Final Project — Part 4                  ║");
  console.log("╚══════════════════════════════════════════════════════════╝");
  console.log(`\nTarget API: ${BASE_URL}`);

  // ── 1. CRUD: GET all vehicles ─────────────────────────────────────────────
  await callEndpoint("Get all vehicles (CRUD — GET /vehicles)", "/vehicles");

  // ── 2. CRUD: POST a new vehicle ───────────────────────────────────────────
  const newId = await createVehicle();

  // ── 3. Non-CRUD: Fleet statistics ────────────────────────────────────────
  await callEndpoint(
    "Fleet statistics (non-CRUD — GET /vehicles/stats)",
    "/vehicles/stats"
  );

  // ── 4. Non-CRUD: Search by make ───────────────────────────────────────────
  await callEndpoint(
    "Search vehicles by make (non-CRUD — GET /vehicles/search?make=Honda)",
    "/vehicles/search?make=Honda"
  );

  console.log(`\n${"═".repeat(60)}`);
  console.log("  All requests complete.");
  console.log(`${"═".repeat(60)}\n`);
}

main();
