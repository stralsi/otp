export async function create(values) {
  const response = await fetch("http://localhost:5000/api/otp", {
    method: "POST",
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(values)
  })

  return response;
}